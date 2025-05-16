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

        private async Task<int> getClassIdByClassIdentity(string classIdentity)
        {
            var classId = await this._context.Classes
                .Select(c => new { c.ClassIdentity, c.ClassId })
                .SingleAsync(c => c.ClassIdentity == classIdentity);

            return classId.ClassId;
        }

        private async Task<TableType> getTableTypeByTypeAsync(string type)
        {
            var tableType = await this._context.TableTypes
                .SingleAsync(tt => tt.Type == type);

            return tableType;
        }

        private async Task<List<int>> getReadRecordsByUserIdByClassIdByTypeAsync(TableType type, string userId, int classId)
        {
            var readRecords = await this._context.UsersViews
                .Where(uv => uv.TableType == type && uv.UserId == userId && uv.ClassId == classId)
                .Select(uv => uv.TableTypeRecordId)
                .ToListAsync();

            return readRecords;
        }

        private async Task<List<int>> getReadRecordsByUserIdByClassIdByTypeAsync(string type, string userId, int classId)
        {
            var tableType = await this.getTableTypeByTypeAsync(type);
            var readRecords = await this._context.UsersViews
                .Where(uv => uv.TableType == tableType && uv.UserId == userId && uv.ClassId == classId)
                .Select(uv => uv.TableTypeRecordId)
                .ToListAsync();

            return readRecords;
        }

        // Main Methods

        public async Task<int> GetUnreadNotificationsCountByUserIdAsync(string userId, string classIdentity)
        {
            var classId = await this.getClassIdByClassIdentity(classIdentity);
            var readNotifications = await this.getReadRecordsByUserIdByClassIdByTypeAsync("Notifications", userId, classId);

            var notificationsCount = await this._context.Notifications
                .CountAsync(n => !readNotifications.Contains(n.NotificationId) && n.ClassId == classId);

            return notificationsCount;
        }

        public async Task ReadAllNotificationsAsync(User user, string classIdentity)
        {
            var classId = await this.getClassIdByClassIdentity(classIdentity);
            var type = await this.getTableTypeByTypeAsync("Notifications");
            var readNotifications = await this.getReadRecordsByUserIdByClassIdByTypeAsync(type, user.Id, classId);

            var unreadNotifications = await this._context.Notifications
                .Where(n => !readNotifications.Contains(n.NotificationId) && n.ClassId == classId)
                .Select(n => new UserView()
                {
                    User = user,
                    UserId = user.Id,
                    TableType = type,
                    TableTypeId = type.TypeId,
                    TableTypeRecordId = n.NotificationId,
                    ClassId = classId
                })
                .ToListAsync();

            await this._context.UsersViews
                .AddRangeAsync(unreadNotifications);

            await this._context.SaveChangesAsync();
        }
    }
}
