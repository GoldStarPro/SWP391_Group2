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
  public class UnitController : Controller
  {
    private readonly HRManagementContext _context;

    public UnitController(HRManagementContext context)
    {
      _context = context;
    }

    // GET: Unit
    public async Task<IActionResult> Index()
    {
      return View(await _context.Units.ToListAsync());
    }

    // GET: Unit/Details/:id
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var tblUnit = await _context.Units
          .FirstOrDefaultAsync(m => m.Unit_ID == id);
      if (tblUnit == null)
      {
        return NotFound();
      }

      return View(tblUnit);
    }

    // GET: Unit/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Unit/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Unit_ID,Unit_Name,Notes")] Unit unit)
    {
      if (ModelState.IsValid)
      {
        _context.Add(unit);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(unit);
    }

    // GET: Unit/Edit/:id
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var tblUnit = await _context.Units.FindAsync(id);
      if (tblUnit == null)
      {
        return NotFound();
      }
      return View(tblUnit);
    }

    // POST: Unit/Edit/:id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Unit_ID,Unit_Name,Notes")] Unit unit)
    {
      if (id != unit.Unit_ID)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(unit);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!UnitExists(unit.Unit_ID))
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
      return View(unit);
    }

    // GET: Unit/Delete/:id
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var tblUnit = await _context.Units
          .FirstOrDefaultAsync(m => m.Unit_ID == id);
      if (tblUnit == null)
      {
        return NotFound();
      }

      return View(tblUnit);
    }

    // POST: Unit/Delete/:id
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var unit = await _context.Units.FindAsync(id);
      _context.Units.Remove(unit);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool UnitExists(int id)
    {
      return _context.Units.Any(e => e.Unit_ID == id);
    }
  }
}
