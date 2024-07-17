﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HR_Management.Models;
using OfficeOpenXml;
using System.IO;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HR_Management.Controllers
{
    public class SalaryStatisticController : Controller
    {
        private readonly HRManagementContext _context;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;

        public SalaryStatisticController(HRManagementContext context, IConverter converter, ICompositeViewEngine viewEngine)
        {
            _context = context;
            _converter = converter;
            _viewEngine = viewEngine;
        }

        // GET: SalaryStatistic
        public async Task<IActionResult> Index()
        {
            var hrmanagementContext = _context.SalaryStatistics.Include(t => t.EmployeeIDNavigation).Include(t => t.MonthIDNavigation);
            return View(await hrmanagementContext.ToListAsync());
        }

        // GET: SalaryStatistic/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryStatistic = await _context.SalaryStatistics
                .Include(t => t.EmployeeIDNavigation)
                .Include(t => t.MonthIDNavigation)
                .FirstOrDefaultAsync(m => m.Salary_Statistic_ID == id);
            if (salaryStatistic == null)
            {
                return NotFound();
            }

            return View(salaryStatistic);
        }

        // GET: SalaryStatistic/Create
        public IActionResult Create()
        {
            ViewData["Employee_ID"] = new SelectList(_context.Employees, "Employee_ID", "Full_Name");
            ViewData["Month_ID"] = new SelectList(_context.Months, "Month_ID", "Month_Name");
            return View();
        }

        // POST: SalaryStatistic/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalaryStatistic salaryStatistic)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Employees
                .Include(t => t.TaxIDNavigation)
                .Include(t => t.SalaryIDNavigation)
                .FirstOrDefaultAsync(m => m.Employee_ID == salaryStatistic.Employee_ID);
                if (user != null)
                {
                    if (salaryStatistic.Fine == null)
                    {
                        salaryStatistic.Fine = 0;
                    }
                    if (salaryStatistic.Bonus == null)
                    {
                        salaryStatistic.Bonus = 0;
                    }
                    salaryStatistic.BasicSalary = user.SalaryIDNavigation.Basic_Salary;
                    salaryStatistic.TaxToPay = user.TaxIDNavigation.Amount;
                    salaryStatistic.TotalSalary = ((salaryStatistic.BasicSalary - salaryStatistic.TaxToPay) + salaryStatistic.Bonus) - salaryStatistic.Fine;
                    salaryStatistic.Create_Date = DateTime.Now;
                    _context.Add(salaryStatistic);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            ViewData["Employee_ID"] = new SelectList(_context.Employees, "Employee_ID", "Full_Name", salaryStatistic.Employee_ID);
            ViewData["Month_ID"] = new SelectList(_context.Months, "Month_ID", "Month_Name", salaryStatistic.Month_ID);
            return View(salaryStatistic);
        }


        // GET: SalaryStatistic/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryStatistic = await _context.SalaryStatistics.FindAsync(id);
            if (salaryStatistic == null)
            {
                return NotFound();
            }
            ViewData["Employee_ID"] = new SelectList(_context.Employees, "Employee_ID", "Employee_ID", salaryStatistic.Employee_ID);
            ViewData["Month_ID"] = new SelectList(_context.Months, "Month_ID", "Month_Name", salaryStatistic.Month_ID);
            return View(salaryStatistic);
        }

        // POST: SalaryStatistic/Edit/:id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalaryStatistic salaryStatistic)
        {
            if (id != salaryStatistic.Salary_Statistic_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Employees
                               .Include(t => t.TaxIDNavigation)
                               .Include(t => t.SalaryIDNavigation)
                               .FirstOrDefaultAsync(m => m.Employee_ID == salaryStatistic.Employee_ID);
                    if (salaryStatistic.Fine == null)
                    {
                        salaryStatistic.Fine = 0;
                    }
                    if (salaryStatistic.Bonus == null)
                    {
                        salaryStatistic.Bonus = 0;
                    }
                    salaryStatistic.BasicSalary = user.SalaryIDNavigation.Basic_Salary;
                    salaryStatistic.TaxToPay = user.TaxIDNavigation.Amount;
                    salaryStatistic.TotalSalary = ((salaryStatistic.BasicSalary - salaryStatistic.TaxToPay) + salaryStatistic.Bonus) - salaryStatistic.Fine;
                    _context.Update(salaryStatistic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryStatisticExists(salaryStatistic.Salary_Statistic_ID))
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
            ViewData["Employee_ID"] = new SelectList(_context.Employees, "Employee_ID", "Employee_ID", salaryStatistic.Employee_ID);
            ViewData["Month_ID"] = new SelectList(_context.Months, "Month_ID", "Month_Name", salaryStatistic.Month_ID);
            return View(salaryStatistic);
        }

        // GET: SalaryStatistic/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaryStatistic = await _context.SalaryStatistics
                .Include(t => t.EmployeeIDNavigation)
                .Include(t => t.MonthIDNavigation)
                .FirstOrDefaultAsync(m => m.Salary_Statistic_ID == id);
            if (salaryStatistic == null)
            {
                return NotFound();
            }

            return View(salaryStatistic);
        }

        // POST: SalaryStatistic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaryStatistic = await _context.SalaryStatistics.FindAsync(id);
            _context.SalaryStatistics.Remove(salaryStatistic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryStatisticExists(int id)
        {
            return _context.SalaryStatistics.Any(e => e.Salary_Statistic_ID == id);
        }


        // Export Salary Statistic List to Excel Format
        public async Task<IActionResult> ExportToExcel(int monthID)
        {
            var data = new List<SalaryStatistic>();
            if (monthID == 0)
            {
                // Lấy dữ liệu từ Entity Framework
                data = await _context.SalaryStatistics
                          .Include(t => t.EmployeeIDNavigation)
                          .Include(t => t.MonthIDNavigation)
                          .ToListAsync();
            }
            else
            {
                // Lấy dữ liệu từ Entity Framework
                data = await _context.SalaryStatistics
                           .Include(t => t.EmployeeIDNavigation)
                           .Include(t => t.MonthIDNavigation)
                           .Where(t => t.Month_ID == monthID)
                           .ToListAsync();
            }

            // Tạo một file Excel mới
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            // Tạo một worksheet mới
            var worksheet = package.Workbook.Worksheets.Add("Salary Statistics");

            // Thêm tiêu đề "Salary Statistic List"
            worksheet.Cells[1, 1, 1, 10].Merge = true;
            worksheet.Cells[1, 1].Value = "Salary Statistic List";
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.Green);
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Row(1).Height = 30;

            // Thêm tiêu đề cho các cột
            worksheet.Cells[2, 1].Value = "No.";
            worksheet.Cells[2, 2].Value = "Month";
            worksheet.Cells[2, 3].Value = "Employee ID";
            worksheet.Cells[2, 4].Value = "Employee Name";
            worksheet.Cells[2, 5].Value = "Email";
            worksheet.Cells[2, 6].Value = "Basic Salary";
            worksheet.Cells[2, 7].Value = "Tax To Pay";
            worksheet.Cells[2, 8].Value = "Bonus";
            worksheet.Cells[2, 9].Value = "The Realistic Total";
            worksheet.Cells[2, 10].Value = "Notes";

            // Định dạng tiêu đề các cột: căn giữa, in đậm, nền xám nhạt
            using (var range = worksheet.Cells[2, 1, 2, 10])
            {
                range.Style.Font.Bold = true;
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            // Thêm dữ liệu vào các cột
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i + 3, 1].Value = i + 1;
                worksheet.Cells[i + 3, 2].Value = data[i].MonthIDNavigation.Month_Name;
                worksheet.Cells[i + 3, 3].Value = data[i].Employee_ID;
                worksheet.Cells[i + 3, 4].Value = data[i].EmployeeIDNavigation.Full_Name;
                worksheet.Cells[i + 3, 5].Value = data[i].EmployeeIDNavigation.Email;
                worksheet.Cells[i + 3, 6].Value = data[i].BasicSalary;
                worksheet.Cells[i + 3, 7].Value = data[i].TaxToPay;
                worksheet.Cells[i + 3, 8].Value = data[i].Bonus;
                worksheet.Cells[i + 3, 9].Value = data[i].TotalSalary;
                worksheet.Cells[i + 3, 10].Value = data[i].Notes;

                // Định dạng nền cho các hàng dữ liệu (chẵn lẻ khác nhau)
                var rowRange = worksheet.Cells[i + 3, 1, i + 3, 10];
                if ((i + 3) % 2 == 0)
                {
                    rowRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.WhiteSmoke);
                }
                else
                {
                    rowRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    rowRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                }

                // Định dạng viền cho các ô dữ liệu
                rowRange.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                rowRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                rowRange.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                rowRange.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            // Tự động căn chỉnh độ rộng các cột
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Save file Excel
            var stream = new MemoryStream();
            package.SaveAs(stream);

            // Trả về file Excel như một phản hồi HTTP
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalaryStatistics_All.xlsx");
        }


        // Export Salary Statistic List to PDF Format
        public async Task<IActionResult> ExportToPDF()
        {
            // Lấy dữ liệu từ Entity Framework
            var data = await _context.SalaryStatistics
                .Include(t => t.EmployeeIDNavigation)
                .Include(t => t.MonthIDNavigation)
                .ToListAsync();

            var htmlContent = RenderViewToString("SalaryStatisticListPDF", data);

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
                Margins = new MarginSettings() { Top = 10 }
            },
                Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" }
                }
            }
            };

            var pdfFile = _converter.Convert(pdf);

            return File(pdfFile, "application/pdf", "SalaryStatistics_All.pdf");
        }

        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext).Wait();
                return sw.GetStringBuilder().ToString();
            }
        }


    }
}
