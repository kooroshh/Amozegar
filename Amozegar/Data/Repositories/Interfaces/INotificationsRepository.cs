using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface INotificationsRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<NotificationsViewModel>> GetNotificationsByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber);
        Task<Notification?> GetNotificationByIdAndClassIdentityAsync(string classIdentity, int notificationId);
        Task<NotificationsDetailsViewModel?> GetNotificationWithPicturesByIdAndClassIdentityAsync(string classIdentity, int notificationId);
        Task<IEnumerable<NotificationsDetailsViewModel>> GetNotificationsWithPicturesByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber);
        Task<int> GetNotificationsCountByClassIdentityAsync(string classIdentity);
    }
}
