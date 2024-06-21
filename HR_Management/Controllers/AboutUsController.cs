using HR_Management.Models;
using Microsoft.AspNetCore.Mvc;

namespace HR_Management.Controllers
{
    public class AboutUsController : Controller
    {

        private readonly HRManagementContext _context;

        public AboutUsController(HRManagementContext context)
        {
            _context = context;
        }

        public IActionResult Hoang()
        {
            // Code để lấy thông tin của Trần Huy Hoàng và trả về View
            return View();
        }

        public IActionResult Sinh()
        {
            // Code để lấy thông tin của Phan Phương Sinh và trả về View
            return View();
        }

        public IActionResult Chien()
        {
            // Code để lấy thông tin của Nguyễn Ngô Chiến và trả về View
            return View();
        }

        public IActionResult Thy()
        {
            // Code để lấy thông tin của Lê Việt Thy và trả về View
            return View();
        }

        public IActionResult Dai()
        {
            // Code để lấy thông tin của Phan Quốc Đại và trả về View
            return View();
        }
    }
}
