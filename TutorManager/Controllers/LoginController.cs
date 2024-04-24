using Microsoft.AspNetCore.Mvc;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{
/*    [ApiController]
    [Route("api/[controller]/[action]")]*/

    /// <summary>
    /// Kontroler do obsługi logowania
    /// </summary>
    public class LoginController : Controller
    {
        private readonly DataContext _db_con;
        private readonly ISession _session;

        /// <summary>
        /// Konstruktor kontrolera logowania
        /// </summary>
        /// <param name="dbContext">Context Entity framework</param>
        /// <param name="httpContextAccessor">Bierząca sesja</param>
        public LoginController(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db_con = dbContext;
            _session = httpContextAccessor.HttpContext.Session;
        }

        /// <summary>
        /// Widok główny logowania
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logowanie
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="password">Hasło</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if(ModelState.IsValid) 
            { 
                var studentData = _db_con.StudentTable.Where(u => u.Email.Equals(email) && u.Password.Equals(password)).ToList(); 
                var tutorData = _db_con.TutorTable.Where(u => u.Email.Equals(email) && u.Password.Equals(password)).ToList();
                //data = _db_con.StudentTable.Where(u => u.Email.Equals(email) && u.Password.Equals(password)).ToList();
                if (studentData.Count() != 0) 
                {
                    _session.SetInt32("StudentID", studentData[0].Id);
                    _session.SetString("FirstName", studentData[0].FirstName);
                    _session.SetString("LastName", studentData[0].LastName);
                    _session.SetString("UserEmail", email);
                    _session.SetString("Phone", studentData[0].PhoneNumber);
                    _session.SetString("Password", studentData[0].Password);
                    _session.SetInt32("Charge", (studentData[0].Charge));
                    return RedirectToAction("Index", "Student");
                }
                else if(tutorData.Count() != 0)
                {
                    _session.SetInt32("TutorID", tutorData[0].Id);
                    _session.SetString("FirstName", tutorData[0].FirstName);
                    _session.SetString("LastName", tutorData[0].LastName);
                    _session.SetString("UserEmail", email);
                    _session.SetString("Phone", tutorData[0].PhoneNumber);
                    _session.SetString("Password", tutorData[0].Password);
                    _session.SetInt32("SumGratification", tutorData[0].SumGratification);
                    _session.SetInt32("NumOfStudents", Convert.ToInt32(tutorData[0].NumOfStudents));
                    _session.SetInt32("ExpGratification", tutorData[0].ExpectedGratification);
                    return RedirectToAction("Index", "Tutor");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(email, password);
        }

        /// <summary>
        /// Wylogowywanie
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            _session.Clear();
            return RedirectToAction("Index");
        }
    }
}
