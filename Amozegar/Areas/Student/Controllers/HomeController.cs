using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Student.Controllers
{
    [Route("Panel/Student/{classId}")]

    public class HomeController : BaseController
    {
        public IActionResult Index(string classId)
        {
            return View();
        }
    }
}
