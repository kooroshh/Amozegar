using Amozegar.Data.UnitOfWork;
using Amozegar.Models.CustomAnnotations;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Student.Controllers
{
    [Route("Panel/Student/{classId}")]
    public class HomeController : BaseController
    {
        private IUnitOfWork _context;

        public HomeController(IUnitOfWork context)
        {
            this._context = context;
        }

        // Main Methods


        public IActionResult Index(string classId)
        {
            ViewBag.Route = "Dashboard";
            return View();
        }


    }
}
