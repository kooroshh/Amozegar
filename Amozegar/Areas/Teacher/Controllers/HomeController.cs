using System.Linq;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/{classId}")]
    public class HomeController : BaseController
    {

        private IUnitOfWork _context;

        public HomeController(
            IUnitOfWork context
        )
        {
            this._context = context;
        }


        public IActionResult Index(string classId)
        {
            ViewBag.Route = "Dashboard";
            return View();
        }

        [Route("Logins-Requests")]
        public async Task<IActionResult> LoginsToClass(string classId)
        {
            var students = await this._context.ClassesRepository.GetClassStudentsRequests(classId);
            ViewBag.Route = "LoginsToClass";
            return View(students);
        }

    }
}
