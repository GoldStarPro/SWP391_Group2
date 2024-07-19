using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace HR_Management.Controllers
{
    [Authorize]
    public class HomeEmployeeController : Controller
    {
        private readonly HRManagementContext _context;

        public HomeEmployeeController(HRManagementContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> PersonalIncomeTax()
        {
            var SessionUserId = HttpContext.Session.GetString("employee_id");
            if (SessionUserId == null)
            {
                return RedirectToAction("Login", "Home"); // Chuyển hướng nếu chưa đăng nhập
            }
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

        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> SocialInsurance()
        {
            var SessionUserId = HttpContext.Session.GetString("employee_id");
            if (SessionUserId == null)
            {
                return RedirectToAction("Login", "Home"); 
            }
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

        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> SalaryStatistic()
        {
            var SessionUserId = HttpContext.Session.GetString("employee_id");
            if (SessionUserId == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var id = int.Parse(SessionUserId);
            var salaryStatistics = await _context.SalaryStatistics.Where(sst => sst.Employee_ID == id)
                                .Include(t => t.EmployeeIDNavigation)
                                .Include(t => t.MonthIDNavigation)
                                .ToListAsync();
            return View(salaryStatistics);
        }

        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> SalaryStatisticDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryStatistics = await _context.SalaryStatistics
                .Include(t => t.EmployeeIDNavigation)
                .Include(t => t.MonthIDNavigation)
                .FirstOrDefaultAsync(m => m.Salary_Statistic_ID == id);
            if (salaryStatistics == null)
            {
                return NotFound();
            }

            return View(salaryStatistics);
        }

    }
}
