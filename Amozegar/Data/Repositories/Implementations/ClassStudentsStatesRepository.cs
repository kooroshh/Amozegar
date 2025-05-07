using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassStudentsStatesRepository : GenericRepository<ClassStudentState>, IClassStudentsStatesRepository
    {
        public ClassStudentsStatesRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<ClassStudentState> GetStateByNameAsync(string state)
        {
            var backState = await this._context.ClassesStudentsStates.SingleAsync(css => css.State == state);
            return backState;
        }
    }
}
