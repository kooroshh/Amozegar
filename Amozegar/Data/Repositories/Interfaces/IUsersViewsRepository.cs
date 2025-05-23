using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IUsersViewsRepository : IGenericRepository<UserView>
    {
        Task<int> GetUnreadNotificationsCountByUserIdAsync(string userId, string classIdentity);
        Task ReadAllNotificationsAsync(User user, string classIdentity);
        Task<int> GetUnreadHomeworksCountByUserIdAsync(string userId, string classIdentity);
        Task ReadAllHomeworksAsync(User user, string classIdentity);
    }
}
