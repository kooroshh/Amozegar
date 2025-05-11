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

        private async Task<int> getCounterAsync(int notificationId)
        {
            var picturesPaths = await this._context.PictureRepository
                .GetPicturesPathsByTypeAndRecordIdAsync(notificationId, "Notifications");

            var lastCount = 0;

            foreach (var picture in picturesPaths)
            {
                var startIndex = picture.IndexOf("/") + 1;
                var lastIndex = picture.IndexOf(".");
                var number = int.Parse(picture.Substring(startIndex, lastIndex - startIndex));

                if (number > lastCount)
                {
                    lastCount = number;
                }
            }

            return (lastCount + 1);
        }

        private async Task addImagesAsync(int notificationId, List<IFormFile>? pictures)
        {
            if (pictures != null && pictures.Count() > 0)
            {
                int counter = await this.getCounterAsync(notificationId);
                foreach (var picture in pictures)
                {
                    string fileName = $"{counter}" + Path.GetExtension(picture.FileName);
                    await picture.SaveImage(fileName, "notifications", $"{notificationId}");
                    fileName = $"{notificationId}/" + fileName;
                    await this._context.PictureRepository
                        .AddImagesAsync(fileName, notificationId, "Notifications");

                    counter++;
                }
                await this._context.SaveChangesAsync();
            }
        }

        private IActionResult returnToNotifications()
        {
            return RedirectToAction("Notifications", "Home", new { area = "Teacher", classId = ViewBag.classId, pageNumber = 1 });
        }


        // Picture Methods

        [Route("Delete-Picture/{pictureId}")]
        public async Task<IActionResult> DeletePicture(string classId, int notificationId, int pictureId)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);
            var picture = await this._context.PictureRepository
                .GetPictureByTypeAndRecordIdAndPictureIdAsync(pictureId, notificationId, "Notifications");

            if (notification == null || picture == null)
            {
                return this.returnToNotifications();
            }

            ViewBag.NotificationId = notificationId;

            var pictureModel = new PictureForEditViewModel()
            {
                PictureId = picture.PictureId,
                PicturePath = picture.PicturePath
            };
            return View(pictureModel);
        }

        [HttpPost("Delete-Picture/{pictureId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePicture(string classId, int notificationId, int pictureId, PictureForEditViewModel editPicture)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var notification = await this.getNotificationByIdAsync(notificationId);

            var picture = await this._context.PictureRepository
                .GetPictureByTypeAndRecordIdAndPictureIdAsync(editPicture.PictureId, notificationId, "Notifications");

            if (notification == null || picture == null || pictureId != editPicture.PictureId)
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

            return RedirectToAction("EditNotification", "Notifications", new { area = "Teacher", classId = classId, notificationId = notificationId });
        }

        [Route("Add-Picture")]
        public async Task<IActionResult> AddPicture(string classId, int notificationId)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);

            if (notification == null)
            {
                return this.returnToNotifications();
            }

            ViewBag.NotificationId = notificationId;

            return View();
        }

        [HttpPost("Add-Picture")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPicture(string classId, int notificationId, AddPictureViewModel addPicture)
        {
            if (!ModelState.IsValid)
            {
                return View(addPicture);
            }

            var notification = await this.getNotificationByIdAsync(notificationId);

            if (notification == null)
            {
                return this.returnToNotifications();
            }

            await this.addImagesAsync(notification.NotificationId, addPicture.Pictures);

            return RedirectToAction("EditNotification", "Notifications", new { area = "Teacher", classId = classId, notificationId = notificationId });
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
        public async Task<IActionResult> AddNotification(string classId, string notificationId, AddOrEditNotificaionsViewModel add)
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

            await this.addImagesAsync(notification.NotificationId, add.Pictures);

            return this.returnToNotifications();
        }


        [Route("Delete")]
        public async Task<IActionResult> DeleteNotification(string classId, int notificationId)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return this.returnToNotifications();
            }
            var delete = new DeleteNotificationViewModel()
            {
                NotificationId = notification.NotificationId,
                NotificationTitle = notification.NotificationTitle
            };
            return View(delete);
        }

        [HttpPost("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteNotification(string classId, int notificationId, DeleteNotificationViewModel delete)
        {
            var notification = await this.getNotificationByIdAsync(notificationId);
            if (
                notificationId != delete.NotificationId ||
                notification == null ||
                !ModelState.IsValid
                )
            {
                return this.returnToNotifications();
            }


            await this._context.PictureRepository.DeleteByTypeAndRecordIdAsync(notificationId, "Notifications");
            var imagesPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "notifications",
                notification.NotificationId.ToString()
                );
            if (Directory.Exists(imagesPath))
            {
                Directory.Delete(imagesPath, recursive: true);
            }

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
                .GetPicturesForEditByTypeAndRecordIdAsync(notificationId, "Notifications");

            ViewBag.NotificationId = notificationId;

            var edit = new AddOrEditNotificaionsViewModel()
            {
                Title = notification.NotificationTitle,
                Body = notification.NotificationBody,
                PicturesList = picturesPaths,
            };
            return View(edit);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNotification(string classId, int notificationId, AddOrEditNotificaionsViewModel edit)
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
