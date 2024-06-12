using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace HR_Management.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly HRManagementContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserDashboardController(HRManagementContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: UserDashboard/Details
        public async Task<IActionResult> Details()
        {
            var SessionUserId = HttpContext.Session.GetString("employee_id");
            var id = int.Parse(SessionUserId);
            var user = await _context.Employees
                .Include(t => t.ExpertiseIDNavigation)
                .Include(t => t.UnitIDNavigation)
                .Include(t => t.QualificationIDNavigation)
                .Include(t => t.SocialInsuranceIDNavigation)
                .Include(t => t.TaxIDNavigation)
                .Include(t => t.SalaryIDNavigation)
                .FirstOrDefaultAsync(m => m.Employee_ID == id);
            return View(user);
        }

        // GET: UserDashboard/Edit/:id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Social_Insurance_ID", employee.Social_Insurance_ID);
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", employee.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_ID", employee.Unit_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", employee.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employee.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_ID", employee.Tax_ID);
            return View(employee);
        }

        // POST: UserDashboard/Edit/:id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Employee_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (employee.ConvertImage != null)
                {
                    string folder = "images/avatar";
                    folder += Guid.NewGuid().ToString() + "_" + employee.ConvertImage.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    await employee.ConvertImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    employee.Image = $"/{folder}";
                }
                else // giữ nguyên giá trị của thuộc tính Image
                {
                    employee.Image = _context.Employees.AsNoTracking().FirstOrDefault(x => x.Employee_ID == employee.Employee_ID)?.Image;
                }
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove("full_name");
                    HttpContext.Session.SetString("full_name", employee.Full_Name.ToString());
                    if (employee.Image != null)
                    {
                        HttpContext.Session.Remove("image");
                        HttpContext.Session.SetString("image", employee.Image.ToString());
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employee.Employee_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details");
            }
            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Social_Insurance_ID", employee.Social_Insurance_ID);
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", employee.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_ID", employee.Unit_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", employee.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employee.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_ID", employee.Tax_ID);
            return View(employee);
        }

        public IActionResult ChangePassWord()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassWord(string oldPassword, string newPassword, string cfPassword)
        {
            var SessionUserId = HttpContext.Session.GetString("employee_id");
            var id = int.Parse(SessionUserId);
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                if (oldPassword == null)
                {
                    ModelState.AddModelError("", "Password cannot be blank!");
                    return View();
                }
                if (newPassword == null)
                {
                    ModelState.AddModelError("", "Password cannot be blank!");
                    return View();
                }
                if (cfPassword == null)
                {
                    ModelState.AddModelError("", "Password cannot be blank!");
                    return View();
                }
                if (employee.Password == oldPassword)
                {
                    if (newPassword == cfPassword)
                    {
                        employee.Password = newPassword;
                        _context.Update(employee);
                        var check = _context.SaveChanges();
                        if (check > 0)
                        {
                            return RedirectToAction("Details");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Error saving data!");
                            return View();
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "The re-enter password does not match!");
                        return View();
                    }

                }
                else
                {
                    ModelState.AddModelError("", "The password is incorrect.");
                    return View();
                }
            }
            else return RedirectToAction("Login", "Home");
        }


        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Employee_ID == id);
        }
    }
}
