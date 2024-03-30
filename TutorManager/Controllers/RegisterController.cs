using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DataContext _db_con;
        public RegisterController(DataContext dbContext)
        {
            _db_con = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

/*        [HttpPost]
        public IActionResult Index(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var check = _db_con.UsersTable.FirstOrDefault(u => u.Email == model.Email);
                if (check == null)
                {
                    _db_con.UsersTable.Add(model);
                    _db_con.SaveChanges();
                    return RedirectToAction("RegistrationSuccess");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View(model);
        }*/

        [HttpPost]
        public IActionResult Index(UserModel model, string UserRole)
        {
            if (ModelState.IsValid)
            {
                if (UserRole == "Student")
                {
                    var student_model = new StudentModel
                    {
                        // Map properties from UserModel to StudentModel
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        ConfirmPassword = model.ConfirmPassword
                    };
                    var check = _db_con.StudentTable.FirstOrDefault(u => u.Email == student_model.Email);
                    if (check == null)
                    {
                        _db_con.StudentTable.Add(student_model);
                        _db_con.SaveChanges();
                        return RedirectToAction("RegistrationSuccess");
                    }
                    else
                    {
                        ViewBag.error = "Email already exists";
                        return View();
                    }
                }
                else if (UserRole == "Tutor")
                {
                    var tutor_model = new TutorModel
                    {
                        // Map properties from UserModel to TutorModel
                        Id = model.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        ConfirmPassword = model.ConfirmPassword
                    };
                    var check = _db_con.TutorTable.FirstOrDefault(u => u.Email == tutor_model.Email);
                    if (check == null)
                    {
                        _db_con.TutorTable.Add(tutor_model);
                        _db_con.SaveChanges();
                        return RedirectToAction("RegistrationSuccess");
                    }
                    else
                    {
                        ViewBag.error = "Email already exists";
                        return View();
                    }
                }
            }
            return View(model);
        }


        public IActionResult RegistrationSuccess()
        {
            return View();
        }

    }
}
