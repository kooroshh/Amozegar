using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    [Route("Panel/Student/{classId}")]
    [ValidateClassIdStudent]
    public class HomeController : Controller
    {
        public IActionResult Index(string classId)
        {
            return View();
        }
    }
}
