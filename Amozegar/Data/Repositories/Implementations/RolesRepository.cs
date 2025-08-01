using Amozegar.Areas.Admin.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class RolesRepository : GenericRepository<UserRole>, IRolesRepository
    {
        public RolesRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EditUserRolesModalViewModel>> GetRolesForEditUserAsync()
        {
            var roels = await this._context.Roles
                .Select(r => new EditUserRolesModalViewModel()
                {
                    PersianName = r.PersianName,
                    RoleId = r.Id
                })
                .ToListAsync();

            return roels;
        }

        public async Task<IEnumerable<UserRole>> GetRolesAsync()
        {
            var roles = await this._context.Roles
                .ToListAsync();

            return roles;
        }
    }
}
