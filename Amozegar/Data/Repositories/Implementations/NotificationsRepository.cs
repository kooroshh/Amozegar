using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;
using Amozegar.Utilities;
using Amozegar.Areas.Shared.Models;

namespace Amozegar.Data.Repositories.Implementations
{
    public class NotificationsRepository : GenericRepository<Notification>, INotificationsRepository
    {
        public NotificationsRepository(AmozegarContext context) : base(context)
        {
        }

        private async Task<int> getClassIdByClassIdentityAsync(string classIdentity)
        {
            var cls = await this._context.Classes
                .Select(c => new { ClassId = c.ClassId, ClassIdentity = c.ClassIdentity })
                .SingleAsync(c => c.ClassIdentity == classIdentity);
            return cls.ClassId;
        }

        public async Task<IEnumerable<NotificationsViewModel>> GetNotificationsByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var classId = await this.getClassIdByClassIdentityAsync(classIdentity);

            var notifications = await this._context.Notifications
                .Where(n => n.ClassId == classId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(n => n.CreatedAt)
                .Select(c => new NotificationsViewModel()
                {
                    NotificationId = c.NotificationId,
                    CreatedAt = c.CreatedAt.ToShamsi(),
                    NotificationTitle = c.NotificationTitle
                })
                .ToListAsync();
            return notifications;
        }

        public async Task<Notification?> GetNotificationByIdAndClassIdentityAsync(string classIdentity, int notificationId)
        {
            var classId = await this.getClassIdByClassIdentityAsync(classIdentity);
            var notification = await this._context.Notifications
                .SingleOrDefaultAsync(n => n.ClassId == classId && n.NotificationId == notificationId);

            return notification;
        }

        public async Task<NotificationsDetailsViewModel?> GetNotificationWithPicturesByIdAndClassIdentityAsync(string classIdentity, int notificationId)
        {
            var notification = await this.GetNotificationByIdAndClassIdentityAsync(classIdentity, notificationId);


            if (notification == null)
            {
                return null;
            }

            var pictureType = await this._context.PictureTypes
                .SingleAsync(pt => pt.Type == "Notifications");


            var pictures = await this._context.Pictures
                .Where(p => p.PictureType == pictureType && p.PictureTypeRecordId == notification.NotificationId)
                .Select(p => p.PicturePath)
                .ToListAsync();

            var notificationDetails = new NotificationsDetailsViewModel()
            {
                NotificationTitle = notification.NotificationTitle,
                CreatedAt = notification.CreatedAt.ToShamsi(),
                NotificationBody = notification.NotificationBody,
                PicturePaths = pictures
            };

            return notificationDetails;
        }

        public async Task<IEnumerable<NotificationsDetailsViewModel>> GetNotificationsWithPicturesByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classId = await this.getClassIdByClassIdentityAsync(classIdentity);
            var notifications = await this._context.Notifications
                .Where(n => n.ClassId == classId)
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(n => new NotificationsDetailsViewModel()
                {
                    CreatedAt = n.CreatedAt.ToShamsi(),
                    NotificationBody = n.NotificationBody,
                    NotificationTitle = n.NotificationTitle,
                    NotificationId = n.NotificationId
                })
                .ToListAsync();

            var pictureType = await this._context.PictureTypes
                .SingleAsync(pt => pt.Type == "Notifications");

            foreach(var notification in notifications)
            {
                notification.PicturePaths = await this._context.Pictures
                .Where(p => p.PictureType == pictureType && p.PictureTypeRecordId == notification.NotificationId)
                .Select(p => p.PicturePath)
                .ToListAsync();
            }




            return notifications;
        }

        public async Task<int> GetNotificationsCountByClassIdentityAsync(string classIdentity)
        {
            var classId = await this.getClassIdByClassIdentityAsync(classIdentity);

            var notificationsCount = await this._context.Notifications
                .CountAsync(n => n.ClassId == classId);

            return notificationsCount;
        }
    }
}
