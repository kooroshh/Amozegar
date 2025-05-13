using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Controllers
{
    [Route("Panel/{roleName}/{classId}/Notifications/{pageNumber}/Show")]
    public class NotificationsController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public NotificationsController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<IActionResult> Index(string classId, string roleName, int pageNumber)
        {
            ViewBag.Route = "ShowNotifications";
            ViewBag.CurrentPage = pageNumber;

            var notifications = await this._context.NotificationsRepository
                .GetNotificationsWithPicturesByClassIdentityByPageNumberAsync(classId, pageNumber);

            if (pageNumber != 1 && notifications.Count() <= 0)
            {
                pageNumber = 1;
                return RedirectToAction("Index", "Notifications", new { classId = classId, roleName = roleName, pageNumber = pageNumber });
            }

            var notificationsCount = await this._context.NotificationsRepository
                    .GetNotificationsCountByClassIdentityAsync(classId);

            var thisPageNotificationsCount = DefaultPageCount.Count * pageNumber;

            if (notificationsCount > thisPageNotificationsCount)
            {
                ViewBag.HasNext = true;
            }

            if (!(thisPageNotificationsCount - 10 <= 0))
            {
                ViewBag.HasPrev = true;
            }

            var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            await this._context.UsersViewsRepository
                .ReadAllNotificationsAsync(user);

            return View(notifications);
        }
    }
}
