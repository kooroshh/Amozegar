using System.Diagnostics;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork context)
        {
            this._unitOfWork = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("About-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("Contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("Contact-us")]
        public async Task<IActionResult> ContactUs(Report report)
        {
            if (!ModelState.IsValid)
            {
                return View(report);
            }

            try
            {
                await this._unitOfWork.ReportRepository.AddAsync(new Report()
                {
                    Email = report.Email,
                    FullName = report.FullName,
                    Message = report.Message,
                    PhoneNumber = report.PhoneNumber,
                    Subject = report.Subject,
                    Date = DateTime.Now
                });
                await this._unitOfWork.SaveChangesAsync();
                ViewBag.State = "Success";
            }
            catch
            {
                ViewBag.State = "Error";
            }
            return View("ContactState");
        }

        [Route("Teacher")]
        public IActionResult Teacher()
        {
            return View();
        }

        [Route("Student")]
        public IActionResult Student()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View("NotFound");
        }
    }
}
