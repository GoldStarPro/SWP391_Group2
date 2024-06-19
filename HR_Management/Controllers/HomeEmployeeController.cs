using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_Management.Models;
using Microsoft.AspNetCore.Http;

namespace HR_Management.Controllers
{
    public class HomeEmployeeController : Controller
    {
        private readonly HRManagementContext _context;

        public HomeEmployeeController(HRManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PersonalIncomeTax()
        {
            var SessionUserId = HttpContext.Session.GetString("employee_id");
            var id = int.Parse(SessionUserId);
            var user = await _context.Employees
                .Include(t => t.ExpertiseIDNavigation)
                .Include(t => t.UnitIDNavigation)
                .Include(t => t.ProjectIDNavigation)
                .Include(t => t.QualificationIDNavigation)
                .Include(t => t.SocialInsuranceIDNavigation)
                .Include(t => t.TaxIDNavigation)
                .Include(t => t.SalaryIDNavigation)
                .FirstOrDefaultAsync(m => m.Employee_ID == id);
            return View(user);
        }

        
    }
}
