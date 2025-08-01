using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Student.Controllers
{
    [Route("Panel/Student/{classId}")]
    public class HomeController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public HomeController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        // Main Methods


        public async Task<IActionResult> Index(string classId)
        {
            ViewBag.Route = "Dashboard";
            var user = await this._userManager.FindByNameAsync(User.Identity.Name);

            var dashboardViewModel = await this._context.DashboardRepository
                .GetStudentDashboardDatasByClassIdentityAsync(classId, user.Id);

            return View(dashboardViewModel);
        }


    }
}
