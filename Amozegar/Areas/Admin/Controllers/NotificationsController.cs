using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin/Notifications")]
    public class NotificationsController : BaseController
    {
        private IUnitOfWork _context;

        public NotificationsController(IUnitOfWork context)
        {
            this._context = context;
        }


        // Utilities

        private IActionResult RedirectToNotifications() => RedirectToAction("Index", "Notifications", new { pageNumber = 1 });
        private IActionResult RedirectToNotifications(string error)
        {
            TempData["Error"] = error;
            return RedirectToAction("Index", "Notifications", new { pageNumber = 1 });
        }

        // Main Methods

        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int pageNumber)
        {
            ViewBag.Route = "Notifications";

            var notifications = await this._context.NotificationsRepository
                .GetNotificationsByPageNumberAsync(pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, notifications.Count()))
            {
                return this.RedirectToNotifications();
            }

            var notificationsCount = await this._context.NotificationsRepository
                .GetNotificationsCountAsync();

            this.checkNextOrPrevForViewBags(notificationsCount, pageNumber);


            return View(notifications);
        }

        [Route("NotificationDetails/{notificationId}")]
        public async Task<IActionResult> NotificationDetails(int notificationId)
        {
            ViewBag.Route = "Notifications";
            var notification = await this._context.NotificationsRepository
                .GetNotificationByIdAsync(notificationId);

            if (notification == null)
            {
                return this.RedirectToNotifications();
            }

            return View(notification);
        }

        [HttpPost("DeleteNotification/{notificationId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            var notification = await this._context.NotificationsRepository
                .GetByIdAsync(notificationId);

            if (notification == null)
            {
                return RedirectToNotifications("چنین اعلانی وجود ندارد");
            }

            var clsIdentity = await this._context.ClassesRepository
                .GetClassIdentityByIdAsync(notification.ClassId);

            await ImageActions.DeleteImages(clsIdentity, notificationId, "Notifications", this._context);

            this._context.NotificationsRepository
                .Delete(notification);

            await this._context.SaveChangesAsync();


            return RedirectToNotifications();
        }


    }
}
