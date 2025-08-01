using Amozegar.Areas.Admin.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Amozegar.Data.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private UserManager<User> _userManager;
        public UserRepository(AmozegarContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<UsersViewModel>> GetUsersByPageNumberAsync(int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var users = await this._context.Users
                .Select(u => new UsersViewModel()
                {
                    Image = u.PicturePath,
                    CreatedAt = u.Date.ToShamsi(),
                    Email = u.Email,
                    UserName = u.FullName,
                    Status = (u.LockoutEnd != null && u.LockoutEnd > DateTime.Now) ? "بن شده" : "فعال",
                    UserId = u.Id,
                    IsBan = (u.LockoutEnd != null && u.LockoutEnd > DateTime.Now)
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var user in users)
            {
                var roles = await _context.UserRoles
                    .Where(ur => ur.UserId == user.UserId)
                    .Join(
                        this._context.Roles,
                        ur => ur.RoleId,
                        r => r.Id,
                        (ur, r) => r.PersianName)
                    .ToListAsync();
                    
                user.Roles = roles;
            }

            return users;

        }

        public Task<int> GetUsersCount()
        {
            var count = this._context.Users.CountAsync();

            return count;
        }
    }
}
