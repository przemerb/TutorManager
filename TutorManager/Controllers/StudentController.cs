using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{

    public class StudentController : Controller
    {
        private readonly DataContext _db_con;
        private readonly ISession _session;
        public StudentController(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db_con = dbContext;
            _session = httpContextAccessor.HttpContext.Session;
        }
        private bool IsLogged()
        {
            return HttpContext.Session.GetString("UserEmail") == null;
        }
        public IActionResult Index()
        {
            if (IsLogged())
            {
                return RedirectToAction("NotLogged", "Home");
            }
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            return View();
        }

        public IActionResult Schedule()
        {
            if (IsLogged())
            {
                return RedirectToAction("NotLogged", "Home");
            }
            return View();
        }

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

        public IActionResult ChangeData()
        {
            return PartialView();
        }

        [HttpPost]
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

        public IActionResult ChangePassword()
        {
            return PartialView();
        }

        [HttpPost]
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

        public IActionResult FindLesson()
        {
            var subjects = _db_con.TutorTable.Select(t => t.Subject).Distinct().ToList();
            ViewBag.Subjects = new SelectList(subjects);

            return View();
        }

        [HttpPost]
        public IActionResult FindLesson(string? selectedSubject)
        {
            var tutors = _db_con.TutorTable.Where(t => t.Subject == selectedSubject).ToList();

            return View("TutorList", tutors);
        }

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

        public IActionResult Lesson()
        {
            var studentID = HttpContext.Session.GetInt32("StudentID");
            var lessons = _db_con.LessonTable.Include(l => l.Tutor).Where(l => l.StudentId == studentID).ToList();
            ViewBag.Lessons = lessons;
            return View();
        }

        public IActionResult Pay()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var student = _db_con.StudentTable.FirstOrDefault(u => u.Email == userEmail);
            return View();
        }

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
    }
}
