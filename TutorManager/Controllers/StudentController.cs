using Microsoft.AspNetCore.Mvc;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{

    public class StudentController : Controller
    {
        private readonly DataContext _db_con;
        public StudentController(DataContext dbContext)
        {
            _db_con = dbContext;
        }
        private bool IsLogged()
        {
            return HttpContext.Session.GetString("UserEmail") == null;
        }
        public IActionResult Index()
        {
            if(IsLogged()) 
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
                    _student.Email = model.Email;
                    _student.FirstName = model.FirstName;
                    _student.LastName = model.LastName;
                    _student.PhoneNumber = model.PhoneNumber;
                    _db_con.SaveChanges();
                    ViewBag.SuccessMessage = "Data updated";
                }
            }
            return View(model);
        }

    }
}
