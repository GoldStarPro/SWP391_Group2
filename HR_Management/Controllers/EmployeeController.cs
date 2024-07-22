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
using Microsoft.Extensions.Logging;

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
            var hr_managementContext = _context.Employees.Include(t => t.SocialInsuranceIDNavigation).Include(t => t.ExpertiseIDNavigation).Include(t => t.UnitIDNavigation).Include(t => t.ProjectIDNavigation).Include(t => t.SalaryIDNavigation).Include(t => t.QualificationIDNavigation).Include(t => t.TaxIDNavigation);
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
                .Include(t => t.ProjectIDNavigation)
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
            ViewData["Project_ID"] = new SelectList(_context.Projects, "Project_ID", "Project_Name");
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
                if (employees.ConvertImage != null)
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

            ViewData["Social_Insurance_ID"] = new SelectList(_context.SocialInsurances, "Social_Insurance_ID", "Registered_Medical_Facility", employees.Social_Insurance_ID);
            ViewData["Expertise_ID"] = new SelectList(_context.Expertises, "Expertise_ID", "Expertise_Name", employees.Expertise_ID);
            ViewData["Unit_ID"] = new SelectList(_context.Units, "Unit_ID", "Unit_Name", employees.Unit_ID);
            ViewData["Project_ID"] = new SelectList(_context.Projects, "Project_ID", "Project_Name", employees.Project_ID);
            ViewData["Salary_ID"] = new SelectList(_context.Salarys, "Salary_ID", "Basic_Salary", employees.Salary_ID);
            ViewData["Qualification_ID"] = new SelectList(_context.Qualifications, "Qualification_ID", "Qualification_Name", employees.Qualification_ID);
            ViewData["Tax_ID"] = new SelectList(_context.PersonalIncomeTaxs, "Tax_ID", "Tax_Authority", employees.Tax_ID);
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
            var hrManageContext = _context.Employees.Include(t => t.SocialInsuranceIDNavigation).Include(t => t.ExpertiseIDNavigation).Include(t => t.UnitIDNavigation).Include(t => t.SalaryIDNavigation).Include(t => t.QualificationIDNavigation).Include(t => t.ProjectIDNavigation).Include(t => t.TaxIDNavigation);
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
        public async Task<IActionResult> Permission(int id, [Bind("Employee_ID,Permission,Full_Name,Date_Of_Birth,Gender,ID_Card_Number,Place_Of_Birth,Address,PhoneNumber,Email,Password")] Employee employee)
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
                return RedirectToAction("PermissionOverview", "Employee");
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
                .Include(t => t.ProjectIDNavigation)
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

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.Employee_ID == id);
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

            // Thêm tiêu đề "Employee List"
            worksheet.Cells[1, 1, 1, 19].Merge = true;
            worksheet.Cells[1, 1].Value = "Employee List";
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.Font.Color.SetColor(System.Drawing.Color.Green);
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Căn giữa theo chiều ngang
            worksheet.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center; // Căn giữa theo chiều dọc
            worksheet.Row(1).Height = 30; // Điều chỉnh chiều cao dòng tiêu đề

            // Thêm tiêu đề cho các cột
            worksheet.Cells[2, 1].Value = "No.";
            worksheet.Cells[2, 2].Value = "Employee ID";
            worksheet.Cells[2, 3].Value = "Full Name";
            worksheet.Cells[2, 4].Value = "Email";
            worksheet.Cells[2, 5].Value = "Phone";
            worksheet.Cells[2, 6].Value = "Gender";
            worksheet.Cells[2, 7].Value = "ID Card Number";
            worksheet.Cells[2, 8].Value = "Place Of Birth";
            worksheet.Cells[2, 9].Value = "Address";
            worksheet.Cells[2, 10].Value = "Ethnicity";
            worksheet.Cells[2, 11].Value = "Religion";
            worksheet.Cells[2, 12].Value = "Nationality";
            worksheet.Cells[2, 13].Value = "Qualification";
            worksheet.Cells[2, 14].Value = "Expertise";
            worksheet.Cells[2, 15].Value = "Unit";
            worksheet.Cells[2, 16].Value = "Registered Medical Facility";
            worksheet.Cells[2, 17].Value = "Tax Authority";
            worksheet.Cells[2, 18].Value = "Basic Salary";
            worksheet.Cells[2, 19].Value = "Notes";

            // Định dạng tiêu đề các cột: căn giữa, in đậm, nền xám nhạt
            using (var range = worksheet.Cells[2, 1, 2, 19])
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
                worksheet.Cells[i + 3, 2].Value = data[i].Employee_ID;
                worksheet.Cells[i + 3, 3].Value = data[i].Full_Name;
                worksheet.Cells[i + 3, 4].Value = data[i].Email;
                worksheet.Cells[i + 3, 5].Value = data[i].PhoneNumber;
                worksheet.Cells[i + 3, 6].Value = data[i].Gender;
                worksheet.Cells[i + 3, 7].Value = data[i].ID_Card_Number;
                worksheet.Cells[i + 3, 8].Value = data[i].Place_Of_Birth;
                worksheet.Cells[i + 3, 9].Value = data[i].Address;
                worksheet.Cells[i + 3, 10].Value = data[i].Ethnicity;
                worksheet.Cells[i + 3, 11].Value = data[i].Religion;
                worksheet.Cells[i + 3, 12].Value = data[i].Nationality;
                worksheet.Cells[i + 3, 13].Value = data[i].QualificationIDNavigation.Qualification_Name;
                worksheet.Cells[i + 3, 14].Value = data[i].ExpertiseIDNavigation.Expertise_Name;
                worksheet.Cells[i + 3, 15].Value = data[i].UnitIDNavigation.Unit_Name;
                worksheet.Cells[i + 3, 16].Value = data[i].SocialInsuranceIDNavigation.Registered_Medical_Facility;
                worksheet.Cells[i + 3, 17].Value = data[i].TaxIDNavigation.Tax_Authority;
                worksheet.Cells[i + 3, 18].Value = data[i].SalaryIDNavigation.Basic_Salary;
                worksheet.Cells[i + 3, 19].Value = data[i].Notes;

                // Định dạng nền cho các hàng dữ liệu (chẵn lẻ khác nhau)
                var rowRange = worksheet.Cells[i + 3, 1, i + 3, 19];
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
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees_All.xlsx");
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

            var htmlContent = RenderViewToString("EmployeeListPDF", data);

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

    }
}
