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
    public class ExpertiseController : Controller
    {
        private readonly HRManagementContext _context;

        public ExpertiseController(HRManagementContext context)
        {
            _context = context;
        }

        // GET: Expertise
        public async Task<IActionResult> Index()
        {
            return View(await _context.Expertises.ToListAsync());
        }

        // GET: ChuyenMon/Details/:id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertises
                .FirstOrDefaultAsync(m => m.Expertise_ID == id);
            if (expertise == null)
            {
                return NotFound();
            }

            return View(expertise);
        }

        // GET: Expertise/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Expertise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Expertise_ID,Expertise_Name,Notes")] Expertise expertise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expertise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expertise);
        }

        // GET: Expertise/Edit/:id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertises.FindAsync(id);
            if (expertise == null)
            {
                return NotFound();
            }
            return View(expertise);
        }

        // POST: Expertise/Edit/:id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Expertise_ID,Expertise_Name,Notes")] Expertise expertise)
        {
            if (id != expertise.Expertise_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expertise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpertiseExists(expertise.Expertise_ID))
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
            return View(expertise);
        }

        // GET: Expertise/Delete/:id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertises
                .FirstOrDefaultAsync(m => m.Expertise_ID == id);
            if (expertise == null)
            {
                return NotFound();
            }

            return View(expertise);
        }

        // POST: Expertise/Delete/:id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expertise = await _context.Expertises.FindAsync(id);
            _context.Expertises.Remove(expertise);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpertiseExists(int id)
        {
            return _context.Expertises.Any(e => e.Expertise_ID == id);
        }
    }
}