using Microsoft.AspNetCore.Mvc;
using TutorManager.Data;

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
                    return RedirectToAction("Index", "Student");
                }
                else if(tutorData.Count() != 0)
                {
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
