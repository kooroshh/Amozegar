using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class UsersViewsRepository : GenericRepository<UserView>, IUsersViewsRepository
    {


        public UsersViewsRepository(AmozegarContext context) : base(context)
        {
        }

        // Utilities

        private async Task<TableType> getTableTypeByTypeAsync(string type)
        {
            var tableType = await this._context.TableTypes
                .SingleAsync(tt => tt.Type == type);

            return tableType;
        }

        private async Task<List<int>> getReadRecordsByTypeAsync(TableType type, string userId)
        {
            var readRecords = await this._context.UsersViews
                .Where(uv => uv.TableType == type && uv.UserId == userId)
                .Select(uv => uv.TableTypeRecordId)
                .ToListAsync();

            return readRecords;
        }

        private async Task<List<int>> getReadRecordsByTypeAsync(string type, string userId)
        {
            var tableType = await this.getTableTypeByTypeAsync(type);
            var readRecords = await this._context.UsersViews
                .Where(uv => uv.TableType == tableType && uv.UserId == userId)
                .Select(uv => uv.TableTypeRecordId)
                .ToListAsync();

            return readRecords;
        }

        // Main Methods

        public async Task<int> GetUnreadNotificationsCountByUserIdAsync(string userId)
        {

            var readNotifications = await this.getReadRecordsByTypeAsync("Notifications", userId);

            var notificationsCount = await this._context.Notifications
                .Where(n => !readNotifications.Contains(n.NotificationId))
                .CountAsync();

            return notificationsCount;
        }

        public async Task ReadAllNotificationsAsync(User user)
        {
            var type = await this.getTableTypeByTypeAsync("Notifications");
            var readNotifications = await this.getReadRecordsByTypeAsync(type, user.Id);

            var unreadNotifications = await this._context.Notifications
                .Where(n => !readNotifications.Contains(n.NotificationId))
                .Select(n => new UserView()
                {
                    User = user,
                    UserId = user.Id,
                    TableType = type,
                    TableTypeId = type.TypeId,
                    TableTypeRecordId = n.NotificationId
                })
                .ToListAsync();

            await this._context.UsersViews
                .AddRangeAsync(unreadNotifications);

            await this._context.SaveChangesAsync();
        }
    }
}
