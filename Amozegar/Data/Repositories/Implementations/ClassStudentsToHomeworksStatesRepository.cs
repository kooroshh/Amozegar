using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassStudentsToHomeworksStatesRepository : GenericRepository<ClassStudentsToHomeworkState>, IClassStudentsToHomeworksStatesRepository
    {
        public ClassStudentsToHomeworksStatesRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<ClassStudentsToHomeworkState> GetClassStudentsToHomeworksStateByStateAsync(string state)
        {
            var classStudentsToHomeworkState = await this._context.ClassStudentsToHomeworkStates
                .SingleAsync(csths => csths.State == state);

            return classStudentsToHomeworkState;
        }
    }
}
