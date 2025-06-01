using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Mvc;


namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/{classId}/Notifications/{notificationId}")]
    public class NotificationsController : BaseController
    {
        private IUnitOfWork _context;

        public NotificationsController(IUnitOfWork context)
        {
            this._context = context;
        }

        // Utilities

        private async Task<Notification?> getNotificationByIdAsync(int notificationId)
        {
            return await this._context.NotificationsRepository
                .GetNotificationByIdAndClassIdentityAsync(ViewBag.ClassId, notificationId);
        }

        private IActionResult returnToNotifications()
        {
            return RedirectToAction("Notifications", "Home", new { area = "Teacher", classId = ViewBag.classId, pageNumber = 1 });
        }

        private IActionResult returnToNotification(int notificationId)
        {
            return RedirectToAction("EditNotification", "Notifications", new { area = "Teacher", classId = ViewBag.classId, notificationId = notificationId });
        }

        // Picture Methods

        [HttpPost("Delete-Picture/{pictureId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePicture(string classId, int notificationId, int pictureId)
        {
            
            var notification = await this.getNotificationByIdAsync(notificationId);

            var picture = await this._context.PictureRepository
                .GetPictureByClassIdentityByTypeAndRecordIdAndPictureIdAsync(this.classId, pictureId, notificationId, "Notifications");

            if (notification == null || picture == null)
            {
                return this.returnToNotifications();
            }

            var picturePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "notifications",
                picture.PicturePath
                );

            await this._context.PictureRepository.DeleteByIdAsync(picture.PictureId);
            await this._context.SaveChangesAsync();

            if (System.IO.File.Exists(picturePath))
            {
                System.IO.File.Delete(picturePath);
            }

            return this.returnToNotification(notificationId);
        }

        [HttpPost("Add-Picture")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPicture(string classId, int notificationId, AddPictureViewModel addPicture)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);

            if (notification == null)
            {
                return this.returnToNotifications();
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                TempData["Error"] = errors;
                return this.returnToNotification(notificationId);
            }

            if (addPicture.Pictures != null)
            {
                await addPicture.Pictures.SaveImages(this.classId, notification.NotificationId, _context, "Notifications");
            }

            return this.returnToNotification(notificationId);
        }


        // CRUD Methods

        [Route("Details")]
        public async Task<IActionResult> Index(string classId, int notificationId)
        {
            ViewBag.Route = "Notifications";
            var notificationDetails = await this._context.NotificationsRepository
                .GetNotificationWithPicturesByIdAndClassIdentityAsync(classId, notificationId);
            if (notificationDetails == null)
            {
                return this.returnToNotifications();
            }


            return View(notificationDetails);
        }


        [Route("")]
        public IActionResult AddNotification(string classId, string notificationId)
        {
            if (notificationId != "Add")
            {
                return this.returnToNotifications();
            }
            return View();
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNotification(string classId, string notificationId, AddOrEditNotificationsViewModel add)
        {


            if (notificationId != "Add")
            {
                return this.returnToNotifications();
            }

            var cls = await this._context.ClassesRepository
                .GetByClassIdentityAsync(classId);

            if (!ModelState.IsValid)
            {
                return View(add);
            }

            var notification = new Notification()
            {
                NotificationTitle = add.Title,
                NotificationBody = add.Body,
                ClassId = cls.ClassId
            };

            await this._context.NotificationsRepository
                .AddAsync(notification);
            await this._context.SaveChangesAsync();

            if (add.Pictures != null)
            {
                await add.Pictures.SaveImages(this.classId, notification.NotificationId, _context, "Notifications");
            }

            return this.returnToNotifications();
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(string classId, int notificationId)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return this.returnToNotifications();
            }

            await ImageActions.DeleteImages(this.classId, notificationId, "Notifications", this._context);

            this._context.NotificationsRepository
                .Delete(notification);

            await this._context.SaveChangesAsync();

            return this.returnToNotifications();
        }



        [Route("Edit")]
        public async Task<IActionResult> EditNotification(string classId, int notificationId)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return this.returnToNotifications();
            }


            var picturesPaths = await this._context.PictureRepository
                .GetPicturesForEditByClassIdentityByTypeAndRecordIdAsync(this.classId, notificationId, "Notifications");

            ViewBag.NotificationId = notificationId;

            var edit = new AddOrEditNotificationsViewModel()
            {
                Title = notification.NotificationTitle,
                Body = notification.NotificationBody,
                PicturesList = picturesPaths,
            };
            return View(edit);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNotification(string classId, int notificationId, AddOrEditNotificationsViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            var notification = await this.getNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return this.returnToNotifications();
            }

            notification.NotificationBody = edit.Body;
            notification.NotificationTitle = edit.Title;
            this._context.NotificationsRepository.Update(notification);
            await this._context.SaveChangesAsync();

            return this.returnToNotifications();
        }

    }
}
