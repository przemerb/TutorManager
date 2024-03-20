using Microsoft.AspNetCore.Mvc;

namespace TutorManager.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
