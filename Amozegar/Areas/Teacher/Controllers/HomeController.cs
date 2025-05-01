using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    [Route("Panel/Teacher/{classId}")]
    [ValidateClassIdTeacher]
    public class HomeController : Controller
    {
        public IActionResult Index(string classId)
        {
            ViewBag.Route = "Dashboard";
            return View();
        }

    }
}
