using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using TutorManager.Data;
using TutorManager.Models;

namespace TutorManager.Controllers
{
    public class RegisterController : Controller
    {
        private readonly DataContext _db_con;
        private readonly ISession _session;
        public RegisterController(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _db_con = dbContext;
            _session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult Index()
        {
            return View();
        }

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
                        ConfirmPassword = model.ConfirmPassword,
                        Charge = 0
                    };
                    var check = _db_con.StudentTable.FirstOrDefault(u => u.Email == student_model.Email);
                    if (check == null)
                    {
                        _db_con.StudentTable.Add(student_model);
                        _db_con.SaveChanges();
                        _session.SetInt32("StudentID", student_model.Id);
                        _session.SetString("FirstName", student_model.FirstName);
                        _session.SetString("LastName", student_model.LastName);
                        _session.SetString("UserEmail", student_model.Email);
                        _session.SetString("Phone", student_model.PhoneNumber);
                        _session.SetString("Password", student_model.Password);
                        _session.SetInt32("Charge", student_model.Charge);
                        return RedirectToAction("Index", "Student");
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
                        ConfirmPassword = model.ConfirmPassword,
                        SumGratification = 0,
                        NumOfStudents = 0

                    };
                    var check = _db_con.TutorTable.FirstOrDefault(u => u.Email == tutor_model.Email);
                    if (check == null)
                    {
                        _db_con.TutorTable.Add(tutor_model);
                        _db_con.SaveChanges();
                        _session.SetInt32("TutorID", tutor_model.Id);
                        _session.SetString("FirstName", tutor_model.FirstName);
                        _session.SetString("LastName", tutor_model.LastName);
                        _session.SetString("UserEmail", tutor_model.Email);
                        _session.SetString("Phone", tutor_model.PhoneNumber);
                        _session.SetString("Password", tutor_model.Password);
                        _session.SetInt32("SumGratification", tutor_model.SumGratification);
                        _session.SetInt32("NumOfStudents", Convert.ToInt32(tutor_model.NumOfStudents));
                        _session.SetInt32("ExpGratification", tutor_model.ExpectedGratification);
                        return RedirectToAction("MoreInfo");
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

        public IActionResult MoreInfo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MoreInfo(TutorModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                var _tutor = _db_con.TutorTable.FirstOrDefault(u => u.Email == userEmail);

                if (_tutor != null)
                {
                    _tutor.Subject = model.Subject;
                    _tutor.ExpectedGratification = model.ExpectedGratification;
                    _db_con.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Tutor");
        }
    }
}
