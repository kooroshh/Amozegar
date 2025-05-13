using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IUsersViewsRepository : IGenericRepository<UserView>
    {
        Task<int> GetUnreadNotificationsCountByUserIdAsync(string userId);
        Task ReadAllNotificationsAsync(User user);
    }
}
