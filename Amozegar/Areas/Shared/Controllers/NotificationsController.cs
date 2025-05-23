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
            ViewBag.Route = "Notifications";

            var notifications = await this._context.NotificationsRepository
                .GetNotificationsWithPicturesByClassIdentityByPageNumberAsync(classId, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, notifications.Count()))
            {
                return this.returnToPaginationView();
            }

            var notificationsCount = await this._context.NotificationsRepository
                    .GetNotificationsCountByClassIdentityAsync(classId);

            this.checkNextOrPrevForViewBags(notificationsCount, pageNumber);

            var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            await this._context.UsersViewsRepository
                .ReadAllNotificationsAsync(user, classId);

            return View(notifications);
        }
    }
}
