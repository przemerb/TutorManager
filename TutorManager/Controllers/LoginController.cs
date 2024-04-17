using Microsoft.AspNetCore.Mvc;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _db_con;
        private readonly ISession _session;
        public LoginController(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db_con = dbContext;
            _session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult Index()
        {
            return View();
        }

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
                    _session.SetString("FirstName", tutorData[0].FirstName);
                    _session.SetString("LastName", tutorData[0].LastName);
                    _session.SetString("UserEmail", email);
                    _session.SetString("Phone", tutorData[0].PhoneNumber);
                    _session.SetString("Password", tutorData[0].Password);
                    _session.SetInt32("SumGratification", tutorData[0].SumGratification);
                    _session.SetInt32("NumOfStudents", Convert.ToInt32(tutorData[0].NumOfStudents));
                    return RedirectToAction("Index", "Tutor");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return View(email, password);
        }

        public ActionResult Logout()
        {
            _session.Clear();
            return RedirectToAction("Index");
        }
    }
}
