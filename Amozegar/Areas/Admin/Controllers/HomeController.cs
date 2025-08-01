using Amozegar.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin")]
    public class HomeController : BaseController
    {
        private IUnitOfWork _context;

        public HomeController(IUnitOfWork context)
        {
            this._context = context;
        }



        public async Task<IActionResult> Index()
        {
            ViewBag.Route = "Dashboard";

            var dashboardViewModel = await this._context.DashboardRepository
                .GetAdminDashboardDatasAsync();

            return View(dashboardViewModel);
        }
    }
}
