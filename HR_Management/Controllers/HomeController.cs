using HR_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly HRManagementContext _context;

        public HomeController(HRManagementContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FirstHomePage()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) == true)
            {
                ModelState.AddModelError("", "Email cannot be blank");
                return View(email);
            }
            if (string.IsNullOrEmpty(password) == true)
            {
                ModelState.AddModelError("", "Password cannot be blank");
                return View(password);
            }
            var user = _context.Employees.SingleOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("employee_id", user.Employee_ID.ToString());
                HttpContext.Session.SetString("full_name", user.Full_Name.ToString());
                if (user.Image != null)
                {
                    HttpContext.Session.SetString("image", user.Image.ToString());
                }
                HttpContext.Session.SetString("email", user.Email.Trim().ToLower());
                HttpContext.Session.SetInt32("role", user.Permission);
                if (user.Permission == 1 || user.Permission == 2)
                {
                    return RedirectToAction("Index");
                }
                else return RedirectToAction("PersonalIncomeTax", "HomeEmployee");
            }
            else
            {
                ModelState.AddModelError("", "Login failed! Check Login Information again!");
                return View();
            }
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Employee_ID,Full_Name,Date_Of_Birth,Gender,ID_Card_Number,Place_Of_Birth,Address,PhoneNumber,Qualification_ID,Social_Insurance_ID,Project_ID,Salary_ID,Unit_ID,Tax_ID,Expertise_ID,Email,Password,Permission,Image,Notes,Ethnicity,Religion,Nationality")] Employee employees)
        {
            if (ModelState.IsValid)
            {
                //if (string.IsNullOrEmpty(employees.Full_Name) == true 
                //    || string.IsNullOrEmpty(employees.Gender) == true 
                //    || string.IsNullOrEmpty(employees.Email) == true 
                //    || employees.PhoneNumber == null 
                //    || employees.Date_Of_Birth == null 
                //    || employees.Password == null)
                //{
                //    ModelState.AddModelError("", "Information cannot be left blank");
                //    return View(employees);
                //}
                var checkEmail = _context.Employees.SingleOrDefault(x => x.Email.Trim().ToLower() == employees.Email.Trim().ToLower());
                if (checkEmail != null)
                {
                    ModelState.AddModelError("", "Email address already exists");
                    return View(employees);
                }
                var checkPhone = _context.Employees.SingleOrDefault(x => x.PhoneNumber == employees.PhoneNumber);
                if (checkPhone != null)
                {
                    ModelState.AddModelError("", "Phone number already exists");
                    return View(employees);
                }
                employees.Permission = 3;
                employees.Qualification_ID = 3;
                employees.Social_Insurance_ID = 1;
                employees.Salary_ID = 3;
                employees.Unit_ID = 1;
                employees.Project_ID = 1;
                employees.Tax_ID = 3;
                employees.Expertise_ID = 1;

                _context.Add(employees);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }

        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("FirstHomePage");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
