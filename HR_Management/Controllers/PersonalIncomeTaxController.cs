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
    public class PersonalIncomeTaxController : Controller
    {
        private readonly HRManagementContext _context;

        public PersonalIncomeTaxController(HRManagementContext context)
        {
            _context = context;
        }

        // GET: PersonalIncomeTax
        public async Task<IActionResult> Index()
        {
            var hrmanagementContext = _context.PersonalIncomeTaxs.Include(t => t.SalaryIDNavigation);
            return View(await hrmanagementContext.ToListAsync());
        }

        // GET: PersonalIncomeTax/Details/:id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalIncomeTaxs = await _context.PersonalIncomeTaxs
                .Include(t => t.SalaryIDNavigation)
                .FirstOrDefaultAsync(m => m.Tax_ID == id);
            if (personalIncomeTaxs == null)
            {
                return NotFound();
            }

            return View(personalIncomeTaxs);
        }

        // GET: PersonalIncomeTax/Create
        public IActionResult Create()
        {
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Basic_Salary");
            return View();
        }

        // POST: PersonalIncomeTax/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Tax_ID,Tax_Authority,Salary_ID,Amount,Registration_Date,Notes")] PersonalIncomeTax personalIncomeTaxs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalIncomeTaxs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["MaLuong"] = new SelectList(_context.TblLuongs, "MaLuong", "MaLuong", tblThueThuNhapCaNhan.MaLuong);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", personalIncomeTaxs.Salary_ID);
            return View(personalIncomeTaxs);
        }

        // GET: PersonalIncomeTax/Edit/:id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalIncomeTaxs = await _context.PersonalIncomeTaxs.FindAsync(id);
            if (personalIncomeTaxs == null)
            {
                return NotFound();
            }
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", personalIncomeTaxs.Salary_ID);
            return View(personalIncomeTaxs);
        }

        // POST: PersonalIncomeTax/Edit/:id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Tax_ID,Tax_Authority,Salary_ID,Amount,Registration_Date,Notes")] PersonalIncomeTax personalIncomeTaxs)
        {
            if (id != personalIncomeTaxs.Tax_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalIncomeTaxs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalIncomeTaxExists(personalIncomeTaxs.Tax_ID))
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
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Salary_ID", personalIncomeTaxs.Salary_ID);
            return View(personalIncomeTaxs);
        }

        // GET: PersonalIncomeTax/Delete/:id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalIncomeTaxs = await _context.PersonalIncomeTaxs
                .Include(t => t.SalaryIDNavigation)
                .FirstOrDefaultAsync(m => m.Tax_ID == id);
            if (personalIncomeTaxs == null)
            {
                return NotFound();
            }

            return View(personalIncomeTaxs);
        }

        // POST: PersonalIncomeTax/Delete/:id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalIncomeTaxs = await _context.PersonalIncomeTaxs.FindAsync(id);
            _context.PersonalIncomeTaxs.Remove(personalIncomeTaxs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalIncomeTaxExists(int id)
        {
            return _context.PersonalIncomeTaxs.Any(e => e.Tax_ID == id);
        }
    }
}
     