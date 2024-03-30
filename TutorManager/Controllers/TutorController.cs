using Microsoft.AspNetCore.Mvc;

namespace TutorManager.Controllers
{
    public class TutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
