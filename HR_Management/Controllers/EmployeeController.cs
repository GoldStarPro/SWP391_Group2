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
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DinkToPdf.Contracts;

namespace HR_Management.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HRManagementContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverter _converter;
        private readonly ICompositeViewEngine _viewEngine;

        public EmployeeController(HRManagementContext context,
            IWebHostEnvironment webHostEnvironment,
            IConverter converter, ICompositeViewEngine viewEngine)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _converter = converter;
            _viewEngine = viewEngine;
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
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("EditUserInfo","Employee");
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

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employees = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToExcel()
        {
            // Lấy dữ liệu từ Entity Framework
            var data = await _context.Employees
                    .Include(t => t.SocialInsuranceIDNavigation)
                    .Include(t => t.ExpertiseIDNavigation)
                    .Include(t => t.UnitIDNavigation)
                    .Include(t => t.SalaryIDNavigation)
                    .Include(t => t.QualificationIDNavigation)
                    .Include(t => t.TaxIDNavigation)
                    .ToListAsync();

            // Tạo một file Excel mới
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            // Tạo một worksheet mới
            var worksheet = package.Workbook.Worksheets.Add("Users");

            // Thêm tiêu đề cho các cột
            worksheet.Cells[1, 1].Value = "No.";
            worksheet.Cells[1, 2].Value = "Employee ID";
            worksheet.Cells[1, 3].Value = "Full Name";
            worksheet.Cells[1, 4].Value = "Email";
            worksheet.Cells[1, 5].Value = "Phone";
            worksheet.Cells[1, 6].Value = "Date Of Birth";
            worksheet.Cells[1, 7].Value = "Gender";
            worksheet.Cells[1, 8].Value = "ID Card Number";
            worksheet.Cells[1, 9].Value = "Place Of Birth";
            worksheet.Cells[1, 10].Value = "Address";
            worksheet.Cells[1, 11].Value = "Ethnicity";
            worksheet.Cells[1, 12].Value = "Religion";
            worksheet.Cells[1, 13].Value = "Nationality";
            worksheet.Cells[1, 14].Value = "Qualification";
            worksheet.Cells[1, 15].Value = "Expertise";
            worksheet.Cells[1, 16].Value = "Unit";
            worksheet.Cells[1, 17].Value = "Registered Medical Facility";
            worksheet.Cells[1, 18].Value = "Tax Authority";
            worksheet.Cells[1, 19].Value = "Basic Salary";
            worksheet.Cells[1, 20].Value = "Notes";

            // Thêm dữ liệu vào các cột
            for (int i = 0; i < data.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = i + 1;
                worksheet.Cells[i + 2, 2].Value = data[i].Employee_ID;
                worksheet.Cells[i + 2, 3].Value = data[i].Full_Name;
                worksheet.Cells[i + 2, 4].Value = data[i].Email;
                worksheet.Cells[i + 2, 5].Value = data[i].PhoneNumber;
                worksheet.Cells[i + 2, 6].Value = data[i].Date_Of_Birth;
                worksheet.Cells[i + 2, 7].Value = data[i].Gender;
                worksheet.Cells[i + 2, 8].Value = data[i].ID_Card_Number;
                worksheet.Cells[i + 2, 9].Value = data[i].Place_Of_Birth;
                worksheet.Cells[i + 2, 10].Value = data[i].Address;
                worksheet.Cells[i + 2, 11].Value = data[i].Ethnicity;
                worksheet.Cells[i + 2, 12].Value = data[i].Religion;
                worksheet.Cells[i + 2, 13].Value = data[i].Nationality;
                worksheet.Cells[i + 2, 14].Value = data[i].QualificationIDNavigation.Qualification_Name;
                worksheet.Cells[i + 2, 15].Value = data[i].ExpertiseIDNavigation.Expertise_Name;
                worksheet.Cells[i + 2, 16].Value = data[i].UnitIDNavigation.Unit_Name;
                worksheet.Cells[i + 2, 17].Value = data[i].SocialInsuranceIDNavigation.Registered_Medical_Facility;
                worksheet.Cells[i + 2, 18].Value = data[i].TaxIDNavigation.Tax_Authority;
                worksheet.Cells[i + 2, 19].Value = data[i].SalaryIDNavigation.Basic_Salary;
                worksheet.Cells[i + 2, 20].Value = data[i].Notes;
            }

            // Save file Excel
            var stream = new MemoryStream();
            package.SaveAs(stream);

            // Trả về file Excel như một phản hồi HTTP
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees_All.xlsx");
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Employee_ID == id);
        }

        // Export Employee List to PDF
        public async Task<IActionResult> ExportToPDF()
        {
            // Lấy dữ liệu từ Entity Framework
            var data = await _context.Employees
                    .Include(t => t.SocialInsuranceIDNavigation)
                    .Include(t => t.ExpertiseIDNavigation)
                    .Include(t => t.UnitIDNavigation)
                    .Include(t => t.SalaryIDNavigation)
                    .Include(t => t.QualificationIDNavigation)
                    .Include(t => t.TaxIDNavigation)
                    .ToListAsync();

            var htmlContent = RenderViewToString("EmployeeListPdf", data);

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

            return File(pdfFile, "application/pdf", "Employees_All.pdf");
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


        //// Version 2 of ExportToPDF() 
        //public IActionResult ExportToPDF()
        //{
        //    // Lấy dữ liệu từ Entity Framework
        //    var data = _context.Employees
        //        .Include(t => t.SocialInsuranceIDNavigation)
        //        .Include(t => t.ExpertiseIDNavigation)
        //        .Include(t => t.UnitIDNavigation)
        //        .Include(t => t.SalaryIDNavigation)
        //        .Include(t => t.QualificationIDNavigation)
        //        .Include(t => t.TaxIDNavigation)
        //        .ToList();

        //    var html = "<html><head><title>Employee List</title></head><body><h1>Employee List</h1><table><tr><th>No.</th><th>Employee ID</th><th>Full Name</th></tr>";
        //    for (int i = 0; i < data.Count; i++)
        //    {
        //        html += $"<tr><td>{i + 1}</td><td>{data[i].Employee_ID}</td><td>{data[i].Full_Name}</td></tr>";
        //    }
        //    html += "</table></body></html>";

        //    var doc = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = {
        //        ColorMode = ColorMode.Color,
        //        Orientation = Orientation.Portrait,
        //        PaperSize = PaperKind.A4,
        //    },
        //        Objects = {
        //        new ObjectSettings() {
        //            PagesCount = true,
        //            HtmlContent = html,
        //            WebSettings = { DefaultEncoding = "utf-8" },
        //            HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //            FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        //        }
        //    }
        //    };

        //    var pdf = _converter.Convert(doc);

        //    return File(pdf, "application/pdf", "EmployeeList.pdf");
        //}

    }
}
