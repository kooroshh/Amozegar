using Amozegar.Areas.Admin.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IRolesRepository : IGenericRepository<UserRole>
    {
        Task<IEnumerable<EditUserRolesModalViewModel>> GetRolesForEditUserAsync();
        Task<IEnumerable<UserRole>> GetRolesAsync();
    }
}
