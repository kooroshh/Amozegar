using Amozegar.Data.UnitOfWork;
using Amozegar.Models.CustomAnnotations;
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

        public IActionResult Index(string classId)
        {
            ViewBag.Route = "Dashboard";
            return View();
        }

        [Route("Students-List")]
        public async Task<IActionResult> StudentsList(string classId)
        {
            ViewBag.Route = "StudentsList";
            var students = await this._context.ClassStudentsRepository
                .GetStudentsByClassIdentityByStateForStudentsAsync(classId, "Accepted");

            return View(students);
        }
    }
}
