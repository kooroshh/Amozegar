using System.Security.Claims;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class HomeworkRepository : GenericRepository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(AmozegarContext context) : base(context)
        {
        }

        // Utilities

        private Task<ClassRoam> getClassByClassIdentityAsync(string classIdentity)
        {
            var cls = this._context.Classes
                .SingleAsync(c => c.ClassIdentity == classIdentity);
            return cls;
        }

        private async Task<HomeworkState> getStateByStateAsync(string state)
        {
            var homeworkState = await this._context.HomeworksStates
                .SingleAsync(hs => hs.State == state);
            return homeworkState;
        }

        // Main Methods

        public async Task<IEnumerable<HomeworksViewModel>> GetHomeworskByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var homeworks = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .Where(h => h.ClassRoam == cls && h.HomeworkState.State != "Deleted")
                .OrderByDescending(h => h.CreatedAt)
                .Skip((page -1) * pageSize)
                .Take(pageSize)
                .Select(h => new HomeworksViewModel()
                {
                    CreatedAt = h.CreatedAt.ToShamsi(),
                    HomewordTitle = h.HomeworkTitle,
                    HomeworkId = h.HomeworkId,
                    State = h.HomeworkState.PersianState
                })
                .ToListAsync();

            return homeworks;
        }

        public async Task<int> GetHomeworksCountByClassIdentityAsync(string classIdentity)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworks = await this._context.Homeworks
                .CountAsync(h => h.ClassRoam == cls && h.HomeworkState.State != "Deleted");

            return homeworks;
        }

        public async Task<ChangeHomeworkStateViewModel?> GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(string classIdentity, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .Where(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState == homeworkState
                )
                .Select(h => new ChangeHomeworkStateViewModel()
                {
                    HomeworkId = h.HomeworkId,
                    HomeworkTitle = h.HomeworkTitle
                })
                .SingleOrDefaultAsync();

            return homework;

        }

        public async Task ChangeHomeworkState(int homeworkId, string state)
        {
            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .SingleAsync(h => h.HomeworkId == homeworkId);

            homework.HomeworkState = homeworkState;
            homework.HomeworkStateId = homeworkState.HomeworkStateId;

            this._context.Homeworks.Update(homework);
        }

        public async Task<ChangeHomeworkStateViewModel?> GetHomeworkByClassIdentityByHomeworkIdByNotThisStateForChangeStateAsync(string classIdentity, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .Where(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState != homeworkState
                )
                .Select(h => new ChangeHomeworkStateViewModel()
                {
                    HomeworkId = h.HomeworkId,
                    HomeworkTitle = h.HomeworkTitle
                })
                .SingleOrDefaultAsync();

            return homework;
        }

        public async Task<Homework?> GetHomeworkByIdByNotThisStateAsync(string classIdentity, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .SingleOrDefaultAsync(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState != homeworkState
                );

            return homework;
        }
    }
}
