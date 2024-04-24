using Azure.Identity;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorManager.Data;

namespace TutorManager.Controllers
{
/*    [ApiController]
    [Route("api/[controller]/[action]")]*/
    /// <summary>
    /// Kontroler panelu Tutora
    /// </summary>
    public class TutorController : Controller
    {
        private readonly DataContext _db_con;
        private readonly ISession _session;

        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar";
        public List<object> GoogleEvents = new List<object>();

        /// <summary>
        /// Konstruktora kontrolera
        /// </summary>
        /// <param name="dbContext">Context Entity framework</param>
        /// <param name="httpContextAccessor"></param>
        public TutorController(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db_con = dbContext;
            _session = httpContextAccessor.HttpContext.Session;
        }

        /// <summary>
        /// Metoda sprawdzająca czy istnieje zalogowany użytkownik w sesji
        /// </summary>
        /// <returns>1 - nie istnieje, 0  - istnieje</returns>
        [NonAction]
        private bool IsLogged()
        {
            return HttpContext.Session.GetString("UserEmail") == null;
        }


        /// <summary>
        /// Widok domowy
        /// </summary>
        /// <returns>Widok domowy</returns>
        [HttpGet]
        public IActionResult Index()
        {
            if (IsLogged())
            {
                return RedirectToAction("NotLogged", "Home");
            }
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            return View();
        }

        /// <summary>
        /// Wyświetlanie informacji o koncie użytkownika
        /// </summary>
        /// <returns>Widok panelu z danymi uzytkowanika</returns>
        [HttpGet]
        public IActionResult Account()
        {
            if (IsLogged())
            {
                return RedirectToAction("NotLogged", "Home");
            }
            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            ViewBag.Phone = HttpContext.Session.GetString("Phone");
            ViewBag.SumGratification = HttpContext.Session.GetInt32("SumGratification");
            ViewBag.ExpGratification = HttpContext.Session.GetInt32("ExpGratification");
            return View();
        }

        /// <summary>
        /// Widok wypłaty
        /// </summary>
        /// <returns>Przekierowanie do panelu konta</returns>
        [HttpGet]
        public IActionResult Withdraw()
        {
            var tutorId = HttpContext.Session.GetInt32("TutorID");
            var tutor = _db_con.TutorTable.FirstOrDefault(t => t.Id == tutorId);

            _db_con.Entry(tutor).Entity.SumGratification = 0;
            _db_con.SaveChanges();
            _session.SetInt32("SumGratification", 0);

            return RedirectToAction("Account");
        }

        /// <summary>
        /// Wyświetlanie harmonogramu
        /// </summary>
        /// <returns>Widok listy HTML</returns>
        [HttpGet]
        public IActionResult Schedule()
        {
            CalendarEvents();
            ViewBag.EventList = GoogleEvents;
            return View();
        }

        /// <summary>
        /// Widok listy zapisanych studnetów
        /// </summary>
        /// <returns>Widok listy HTML</returns>
        [HttpGet]
        public IActionResult Student_list()
        {
            var tutorID = HttpContext.Session.GetInt32("TutorID");
            var students = _db_con.LessonTable
                .Include(l => l.Student)
                .Where(l => l.TutorId == tutorID)
                .Distinct()
                .ToList();

            ViewBag.Students = students;
            return View();
        }

        /// <summary>
        /// Widok lekcji użytkownika
        /// </summary>
        /// <returns>Widok listy HTML</returns>
        [HttpGet]
        public IActionResult Lessons()
        {
            var tutorID = HttpContext.Session.GetInt32("TutorID");
            var inquiries = _db_con.LessonTable
                .Include(l => l.Student)
                .Include(l => l.Tutor)
                .Where(l => l.LessonStatus == "Pending" && l.TutorId == tutorID)
                .ToList();
            ViewBag.Inquiries = inquiries;

            var accepted = _db_con.LessonTable
                .Include(l => l.Student)
                .Include(l => l.Tutor)
                .Where(l => l.LessonStatus == "Accepted" && l.TutorId == tutorID)
                .ToList();
            ViewBag.Accepted = accepted;

            return View();
        }

        /// <summary>
        /// Akceptacja lekcji przez Tutora
        /// </summary>
        /// <param name="lessonId">ID lekcji do akceptacji</param>
        /// <returns>Przekierowanie do widoku lekcji</returns>
        [HttpPost]
        public IActionResult AcceptLesson(int lessonId)
        {
            var lesson = _db_con.LessonTable.FirstOrDefault(l => l.LessonId == lessonId && l.LessonStatus == "Pending");
            var tutor = _db_con.TutorTable.FirstOrDefault(t => t.Id == lesson.TutorId);
            var student = _db_con.StudentTable.FirstOrDefault(s => s.Id == lesson.StudentId);

            if (lesson != null)
            {
                lesson.LessonStatus = "Accepted";
                lesson.Price = tutor.ExpectedGratification + (int)(tutor.ExpectedGratification*0.1);
                lesson.TutorGratification = tutor.ExpectedGratification;
                _db_con.Entry(tutor).Entity.SumGratification += tutor.ExpectedGratification;
                _db_con.Entry(student).Entity.Charge += lesson.Price;
                _db_con.SaveChanges();
            }

            return RedirectToAction("Lessons");
        }

        /// <summary>
        /// Odrzucenie leckji przez Tutora
        /// </summary>
        /// <param name="lessonId">ID lekcji do odrzucenia</param>
        /// <returns>Przekierowanie do widoku lekcji</returns>
        [HttpPost]
        public IActionResult RejectLesson(int lessonId)
        {
            var lesson = _db_con.LessonTable.FirstOrDefault(l => l.LessonId == lessonId && l.LessonStatus == "Pending");

            if (lesson != null)
            {
                lesson.LessonStatus = "Rejected";
                _db_con.SaveChanges();
            }

            return RedirectToAction("Lessons");
        }

        /// <summary>
        /// Wyświetlanie panelu kalendarza z Google Calendar API
        /// </summary>
        [HttpPost]
        public void CalendarEvents()
        {
            UserCredential credential;
            //string path = Server.MapPath("credentail.json");

            try
            {
                using (var stream =
                    new FileStream("Credential2.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                Events events = request.Execute();
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        GoogleEvents.Add(new
                        {
                            Title = eventItem.Summary,
                            Start = eventItem.Start.DateTime,
                            End = eventItem.End.DateTime,
                        });
                    }
                }

                var userEmail = HttpContext.Session.GetString("UserEmail");
                var tutor = _db_con.TutorTable.FirstOrDefault(t => t.Email == userEmail);
                var lessons = _db_con.LessonTable
                    .Include(l => l.Student)
                    .Where(l => l.TutorId == tutor.Id).ToList();

                foreach (var lesson in lessons)
                {
                    GoogleEvents.Add(new
                    {
                        Title = $"Lesson with {lesson.Student.FirstName} {lesson.Student.LastName}",
                        Start = lesson.LessonDateTime,
                        End = lesson.LessonDateTime.AddHours(1),
                    });
                }
                GoogleEvents = GoogleEvents.OrderBy((dynamic e) => e.Start).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}
