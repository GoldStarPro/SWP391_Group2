using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_Management.Models;

namespace HR_Management.Controllers
{
    public class SalaryController : Controller
    {
        private readonly HRManagementContext _context;

        public SalaryController(HRManagementContext context)
        {
            _context = context;
        }

        // GET: Salary
        public async Task<IActionResult> Index()
        {
            var hrmanagementContext = _context.Salarys.Include(t => t.ExpertiseIDNavigation).Include(t => t.UnitIDNavigation).Include(t => t.QualificationIDNavigation);
            return View(await hrmanagementContext.ToListAsync());
        }

        // GET: Salary/Details/:id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salarys
                .Include(t => t.ExpertiseIDNavigation)
                .Include(t => t.UnitIDNavigation)
                .Include(t => t.QualificationIDNavigation)
                .FirstOrDefaultAsync(m => m.Salary_ID == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salary/Create
        public IActionResult Create()
        {
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name");
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name");
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name");
            return View();
        }

        // POST: Salary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Salary_ID,Expertise_ID,Qualification_ID,Unit_ID,Basic_Salary,New_Basic_Salary,Entry_Date,Modify_Date,Notes")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", salary.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name", salary.Unit_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", salary.Qualification_ID);
            return View(salary);
        }

        // GET: Salary/Edit/:id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salarys.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", salary.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name", salary.Unit_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", salary.Qualification_ID);
            return View(salary);
        }

        // POST: Salary/Edit/:id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Salary_ID,Expertise_ID,Qualification_ID,Unit_ID,Basic_Salary,New_Basic_Salary,Entry_Date,Modify_Date,Notes")] Salary salary)
        {
            if (id != salary.Salary_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.Salary_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", salary.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name", salary.Unit_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", salary.Qualification_ID);
            return View(salary);
        }

        // GET: Salary/Delete/:id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblSalary = await _context.Salarys
                .Include(t => t.ExpertiseIDNavigation)
                .Include(t => t.UnitIDNavigation)
                .Include(t => t.QualificationIDNavigation)
                .FirstOrDefaultAsync(m => m.Salary_ID == id);
            if (tblSalary == null)
            {
                return NotFound();
            }

            return View(tblSalary);
        }

        // POST: Salary/Delete/:id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salary = await _context.Salarys.FindAsync(id);
            _context.Salarys.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(int id)
        {
            return _context.Salarys.Any(e => e.Salary_ID == id);
        }
    }
}