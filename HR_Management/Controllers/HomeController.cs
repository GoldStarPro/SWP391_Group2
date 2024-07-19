﻿using HR_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace HR_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly HRManagementContext _context;

        public HomeController(HRManagementContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "AdminPolicy")]
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
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email cannot be blank");
                return View();
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Password cannot be blank");
                return View();
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

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Full_Name),
                    new Claim("Permission", user.Permission.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (user.Permission == 1 || user.Permission == 2)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("PersonalIncomeTax", "HomeEmployee");
                }
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
                employees.Qualification_ID = 2;
                employees.Social_Insurance_ID = 1;
                employees.Salary_ID = 3;
                employees.Unit_ID = 4;
                employees.Project_ID = 1;
                employees.Tax_ID = 1;
                employees.Expertise_ID = 7;

                // Set default image based on gender
                if (string.IsNullOrEmpty(employees.Image))
                {
                    employees.Image = employees.Gender == "Male" ? "/assets/img/boy.png" : "/assets/img/girl.png";
                }

                _context.Add(employees);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Đăng xuất khỏi hệ thống xác thực
            return RedirectToAction("FirstHomePage");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
