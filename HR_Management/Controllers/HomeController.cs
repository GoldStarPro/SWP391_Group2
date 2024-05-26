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
            var user = _context.employees.SingleOrDefault(x => x.Email.Trim().ToLower() == email.Trim().ToLower() && x.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("employee_id", user.Employee_ID.ToString());
                HttpContext.Session.SetString("full_name", user.Full_Name.ToString());
                if (user.Image != null)
                {
                    HttpContext.Session.SetString("image", user.Image.ToString());
                }
                HttpContext.Session.SetString("email", user.Email.Trim().ToLower());
                HttpContext.Session.SetInt32("role", user.Permisson);
                if (user.Permisson == 1 || user.Permisson == 2)
                {
                    return RedirectToAction("Index");
                }
                else return RedirectToAction("PersonalTaxIncome", "HomeEmployee");
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
        public async Task<IActionResult> Register([Bind("employee_id,full_name,date_of_birth,gender,id_card_number,place_of_birth,address,phone_number,qualification_id,social_insurance_id,salary_id,unit_id,tax_id,expertise_id,email,password,permisson,image,notes,ethnicity,religion,nationality")] Employee employees)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(employees.Full_Name) == true 
                    || string.IsNullOrEmpty(employees.Gender) == true 
                    || string.IsNullOrEmpty(employees.Email) == true 
                    || employees.PhoneNumber == null 
                    || employees.Date_Of_Birth == null 
                    || employees.Password == null)
                {
                    ModelState.AddModelError("", "Information cannot be left blank");
                    return View(employees);
                }
                var checkEmail = _context.employees.SingleOrDefault(x => x.Email.Trim().ToLower() == employees.Email.Trim().ToLower());
                if (checkEmail != null)
                {
                    ModelState.AddModelError("", "Email address already exists");
                    return View(employees);
                }
                var checkPhone = _context.employees.SingleOrDefault(x => x.PhoneNumber == employees.PhoneNumber);
                if (checkPhone != null)
                {
                    ModelState.AddModelError("", "Phone number already exists");
                    return View(employees);
                }
                employees.Permisson = 3;
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
            return RedirectToAction("Login");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
