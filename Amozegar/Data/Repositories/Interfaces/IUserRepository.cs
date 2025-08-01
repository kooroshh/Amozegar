using Amozegar.Areas.Admin.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<UsersViewModel>> GetUsersByPageNumberAsync(int pageNumber);
        Task<int> GetUsersCount();
    }
}
