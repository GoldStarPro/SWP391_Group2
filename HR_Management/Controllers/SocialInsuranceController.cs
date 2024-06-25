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
    public class SocialInsuranceController : Controller
    {
        private readonly HRManagementContext _context;

        public SocialInsuranceController(HRManagementContext context)
        {
            _context = context;
        }

        // GET: SocialInsurance
        public async Task<IActionResult> Index()
        {
            return View(await _context.SocialInsurances.ToListAsync());
        }

        // GET: SocialInsurance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialInsurances = await _context.SocialInsurances
                .FirstOrDefaultAsync(m => m.Social_Insurance_ID == id);
            if (socialInsurances == null)
            {
                return NotFound();
            }

            return View(socialInsurances);
        }

        // GET: SocialInsurance/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SocialInsurance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Social_Insurance_ID,Issue_Date,Issue_Place,Registered_Medical_Facility,Notes")] SocialInsurance socialInsurances)
        {
            if (ModelState.IsValid)
            {
                _context.Add(socialInsurances);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(socialInsurances);
        }

        // GET: SocialInsurance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialInsurances = await _context.SocialInsurances.FindAsync(id);
            if (socialInsurances == null)
            {
                return NotFound();
            }
            return View(socialInsurances);
        }

        // POST: SocialInsurance/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Social_Insurance_ID,Issue_Date,Issue_Place,Registered_Medical_Facility,Notes")] SocialInsurance socialInsurances)
        {
            if (id != socialInsurances.Social_Insurance_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(socialInsurances);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SocialInsuranceExists(socialInsurances.Social_Insurance_ID))
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
            return View(socialInsurances);
        }

        // GET: SocialInsurance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var socialInsurances = await _context.SocialInsurances
                .FirstOrDefaultAsync(m => m.Social_Insurance_ID == id);
            if (socialInsurances == null)
            {
                return NotFound();
            }

            return View(socialInsurances);
        }

        // POST: SocialInsurance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var socialInsurances = await _context.SocialInsurances.FindAsync(id);
            _context.SocialInsurances.Remove(socialInsurances);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SocialInsuranceExists(int id)
        {
            return _context.SocialInsurances.Any(e => e.Social_Insurance_ID == id);
        }
    }
}
