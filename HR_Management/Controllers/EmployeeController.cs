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
        public async Task<IActionResult> Index()
        {
            var hr_managementContext = _context.Employees.Include(t => t.SocialInsuranceIDNavigation).Include(t => t.ExpertiseIDNavigation).Include(t => t.UnitIDNavigation).Include(t => t.SalaryIDNavigation).Include(t => t.QualificationIDNavigation).Include(t => t.TaxIDNavigation);
            return View(await hr_managementContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(t => t.SocialInsuranceIDNavigation)
                .Include(t => t.ExpertiseIDNavigation)
                .Include(t => t.UnitIDNavigation)
                .Include(t => t.SalaryIDNavigation)
                .Include(t => t.QualificationIDNavigation)
                .Include(t => t.TaxIDNavigation)
                .FirstOrDefaultAsync(m => m.Employee_ID == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Registered_Medical_Facility");
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name");
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name");
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Basic_Salary");
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name");
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_Authority");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employees)
        {
            if (ModelState.IsValid)
            {
                if (employees.ConvertImage!= null)
                {
                    string folder = "images/avatar";
                    folder += Guid.NewGuid().ToString() + "_" + employees.ConvertImage.FileName;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    await employees.ConvertImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    employees.Image = $"/{folder}";
                }
                _context.Add(employees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Social_Insurance_ID", employees.Social_Insurance_ID);
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", employees.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_ID", employees.Unit_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", employees.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employees.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_ID", employees.Tax_ID);
            return View(employees);
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
            ViewData["Project_ID"] = new SelectList(_context.Projects, "Project_ID", "Project_Name", employee.Project_ID);
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
            ViewData["Project_ID"] = new SelectList(_context.Projects, "Project_ID", "Project_ID", employee.Project_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", employee.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employee.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_ID", employee.Tax_ID);
            return View(employee);
        }

        // GET: Employee
        public async Task<IActionResult> PermissionOverview()
        {
            var hrManageContext = _context.Employees.Include(t => t.SocialInsuranceIDNavigation).Include(t => t.ExpertiseIDNavigation).Include(t => t.UnitIDNavigation).Include(t => t.SalaryIDNavigation).Include(t => t.QualificationIDNavigation).Include(t => t.TaxIDNavigation);
            return View(await hrManageContext.ToListAsync());
        }

        // GET: Employee/Permission/5
        [HttpGet]
        public async Task<IActionResult> Permission(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Employee_ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Permission/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Permission(int id, [Bind("Employee_ID,Permission")] Employee employee)
        {
            if (id != employee.Employee_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Employee_ID == id);
                    if (existingEmployee == null)
                    {
                        return NotFound();
                    }

                    existingEmployee.Permission = employee.Permission;
                    _context.Update(existingEmployee);
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
                return RedirectToAction("PermissionOverview","Employee");
            }
            return View(employee);
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Employee_ID == id);
        }
    }
}
