using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{
/*    [ApiController]
    [Route("api/[controller]/[action]")]*/

    /// <summary>
    /// Kontroler panelu Studenta
    /// </summary>
    public class StudentController : Controller
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
        public StudentController(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
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
        /// Widok harmonogramu
        /// </summary>
        /// <returns>Widok harmonogramu</returns>
        [HttpGet]
        public IActionResult Schedule()
        {
            CalendarEvents();
            ViewBag.EventList = GoogleEvents;
            return View();
        }

        /// <summary>
        /// Widok konta
        /// </summary>
        /// <returns>Dane użytkownika</returns>
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
            ViewBag.Charge = HttpContext.Session.GetInt32("Charge");
            return View();
        }

        /// <summary>
        /// Widok formularza zmiany danych użytkownika
        /// </summary>
        /// <returns>Widok formularza HTML</returns>
        [HttpGet]
        public IActionResult ChangeData()
        {
            return PartialView();
        }


        /// <summary>
        /// Zmiana danych w bazie
        /// </summary>
        /// <param name="model">Model z nowymi danymi</param>
        /// <returns>Przekierowanie po zmianie danych</returns>
        //[HttpPost]
        public IActionResult ChangeData(UserModel model)
        {

            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                var _student = _db_con.StudentTable.FirstOrDefault(u => u.Email == userEmail);

                if (_student != null)
                {
                    _student.FirstName = model.FirstName;
                    _student.LastName = model.LastName;
                    _student.PhoneNumber = model.PhoneNumber;
                    _db_con.SaveChanges();
                    ViewBag.SuccessMessage = "Data updated";
                }
            }
            return View(model);
        }

        /// <summary>
        /// Widok formularza do zmiany hasła
        /// </summary>
        /// <returns>Widok formularza HTML</returns>
        //[HttpGet]
        public IActionResult ChangePassword()
        {
            return PartialView();
        }

        /// <summary>
        /// Zmiana hasła w bazie
        /// </summary>
        /// <param name="model">Model ze zmienionym hasłem</param>
        /// <returns>Widok po zmianie hasła</returns>
        //[HttpPost]
        public IActionResult ChangePassword(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                var _student = _db_con.StudentTable.FirstOrDefault(u => u.Email == userEmail);

                if (_student != null)
                {
                    _student.Password = model.Password;
                    _student.ConfirmPassword = model.ConfirmPassword;
                    _db_con.SaveChanges();
                    ViewBag.SuccessMessage = "Data updated";
                }
            }
            return View();
        }

        /// <summary>
        /// Widok formularza do szukania lekcji
        /// </summary>
        /// <returns>Widok formularza HTML</returns>
        [HttpGet]
        public IActionResult FindLesson()
        {
            var subjects = _db_con.TutorTable.Select(t => t.Subject).Distinct().ToList();
            ViewBag.Subjects = new SelectList(subjects);

            return View();
        }

        /// <summary>
        /// Znajdowanie w bazie Tutorów dla danego przedmiotu
        /// </summary>
        /// <param name="selectedSubject">Przedmiot do wyszukiwania</param>
        /// <returns>Widok listy dostępnych Tutorów</returns>
        [HttpPost]
        public IActionResult FindLesson(string? selectedSubject)
        {
            var tutors = _db_con.TutorTable.Where(t => t.Subject == selectedSubject).ToList();

            return View("TutorList", tutors);
        }

        /// <summary>
        /// Obsługa zapisu na lekcję
        /// </summary>
        /// <param name="tutorId">ID wybranego Tutora</param>
        /// <param name="lessonDateTime">Data podana przez użytkownika</param>
        /// <returns>Przikierowanie do widoku wyszukiwania lekcji</returns>
        [HttpPost]
        public IActionResult RequestLesson(int tutorId, DateTime lessonDateTime)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var student = _db_con.StudentTable.FirstOrDefault(u => u.Email == userEmail);
            var tutor = _db_con.TutorTable.FirstOrDefault(t => t.Id == tutorId);

            if (student != null && tutor != null)
            {
                var newLesson = new LessonModel
                {
                    StudentId = student.Id,
                    TutorId = tutor.Id,
                    Student = student,
                    Tutor = tutor,
                    Subject = tutor.Subject,
                    LessonStatus = "Pending",
                    LessonDateTime = lessonDateTime
                };

                _db_con.LessonTable.Add(newLesson);
                _db_con.SaveChanges();
            }
            return RedirectToAction("FindLesson");
        }

        /// <summary>
        /// Widok lekcji użytkownika
        /// </summary>
        /// <returns>Widok listy HTML</returns>
        [HttpGet]
        public IActionResult Lesson()
        {
            var studentID = HttpContext.Session.GetInt32("StudentID");
            var lessons = _db_con.LessonTable.Include(l => l.Tutor).Where(l => l.StudentId == studentID).ToList();
            ViewBag.Lessons = lessons;
            return View();
        }

        /// <summary>
        /// Panel opłat
        /// </summary>
        /// <returns>Widok panelu opłat</returns>
        [HttpGet]
        public IActionResult Pay()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var student = _db_con.StudentTable.FirstOrDefault(u => u.Email == userEmail);
            ViewBag.Charge = HttpContext.Session.GetInt32("Charge");
            return View();
        }

        /// <summary>
        /// Obsługa zapłaty
        /// </summary>
        /// <returns>Przekierowanie do widoku panelu opłat</returns>
        [HttpGet]
        public IActionResult Payment()
        {
            var student_id = HttpContext.Session.GetInt32("StudentID");
            var student = _db_con.StudentTable.FirstOrDefault(s => s.Id == student_id);

            ViewBag.Charge = HttpContext.Session.GetInt32("Charge");

            _db_con.Entry(student).Entity.Charge = 0;
            _db_con.SaveChanges();
            _session.SetInt32("Charge", student.Charge);
            return RedirectToAction("Pay");
        }

        /// <summary>
        /// Wyświetlanie panelu kalendarza z Google Calendar API
        /// </summary>
        [HttpGet]
        public void CalendarEvents()
        {
            UserCredential credential;
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
