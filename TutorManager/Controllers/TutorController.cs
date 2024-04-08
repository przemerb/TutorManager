using Microsoft.AspNetCore.Mvc;

namespace TutorManager.Controllers
{
    public class TutorController : Controller
    {
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
        public IActionResult Schedule()
        {
            return View();
        }

        public IActionResult Student_list()
        {
            return View();
        }
    }
}
