using System.Diagnostics;
using Amozegar.Data;
using Amozegar.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Controllers
{
    public class HomeController : Controller
    {
        private AmozegarContext _context;
        public HomeController(AmozegarContext context)
        {
            this._context = context;
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
                await this._context.Reports.AddAsync(new Report()
                {
                    Email = report.Email,
                    FullName = report.FullName,
                    Message = report.Message,
                    PhoneNumber = report.PhoneNumber,
                    Subject = report.Subject,
                    Date = DateTime.Now
                });
                this._context.SaveChanges();
                ViewBag.State = "Success";
            }
            catch
            {
                ViewBag.State = "Error";
            }
            return View("ContactState");
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
