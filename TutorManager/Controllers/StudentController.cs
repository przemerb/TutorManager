using Microsoft.AspNetCore.Mvc;

namespace TutorManager.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
