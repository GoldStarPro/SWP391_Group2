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
    public class QualificationController : Controller
    {
        private readonly HRManagementContext _context;

        public QualificationController(HRManagementContext context)
        {
            _context = context;
        }

        // GET: Qualification
        public async Task<IActionResult> Index()
        {
            return View(await _context.Qualifications.ToListAsync());
        }

        // GET: Qualification/Details/:id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qualifications = await _context.Qualifications
                .FirstOrDefaultAsync(m => m.Qualification_ID == id);
            if (qualifications == null)
            {
                return NotFound();
            }

            return View(qualifications);
        }

        // GET: Qualification/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Qualification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Qualification_ID,Qualification_Name,Notes")] Qualification qualification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qualification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qualification);
        }

        // GET: Qualification/Edit/:id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qualification = await _context.Qualifications.FindAsync(id);
            if (qualification == null)
            {
                return NotFound();
            }
            return View(qualification);
        }

        // POST: Qualification/Edit/:id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Qualification_ID,Qualification_Name,Notes")] Qualification qualification)
        {
            if (id != qualification.Qualification_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qualification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QualificationExists(qualification.Qualification_ID))
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
            return View(qualification);
        }

        // GET: TrinhDo/Delete/:id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qualification = await _context.Qualifications
                .FirstOrDefaultAsync(m => m.Qualification_ID == id);
            if (qualification == null)
            {
                return NotFound();
            }

            return View(qualification);
        }

        // POST: Qualification/Delete/:id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qualification = await _context.Qualifications.FindAsync(id);
            _context.Qualifications.Remove(qualification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QualificationExists(int id)
        {
            return _context.Qualifications.Any(e => e.Qualification_ID == id);
        }
    }
}
