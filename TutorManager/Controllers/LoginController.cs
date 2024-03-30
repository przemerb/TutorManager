using Microsoft.AspNetCore.Mvc;
using TutorManager.Data;

namespace TutorManager.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _db_con;
        public LoginController(DataContext dbContext)
        {
            _db_con = dbContext;
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
    }
}
