using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_Management.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;

namespace HR_Management.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HRManagementContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(HRManagementContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        

        // GET: Employee
        public async Task<IActionResult> EditUserInfo()
        {
            var hrManageContext = _context.Employees.Include(t => t.SocialInsuranceIDNavigation).Include(t => t.ExpertiseIDNavigation).Include(t => t.UnitIDNavigation).Include(t => t.SalaryIDNavigation).Include(t => t.QualificationIDNavigation).Include(t => t.TaxIDNavigation);
            return View(await hrManageContext.ToListAsync());
        }


        // GET: Employee/Edit/5
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
            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Registered_Medical_Facility", employee.Social_Insurance_ID);
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", employee.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name", employee.Unit_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Basic_Salary", employee.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employee.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_Authority", employee.Tax_ID);
            return View(employee);
        }

        // POST: Employee/Edit/:id
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
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("EditUserInfo","Employee");
            }
            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Social_Insurance_ID", employee.Social_Insurance_ID);
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", employee.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_ID", employee.Unit_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", employee.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employee.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_ID", employee.Tax_ID);
            return View(employee);
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Employee_ID == id);
        }
    }
}
