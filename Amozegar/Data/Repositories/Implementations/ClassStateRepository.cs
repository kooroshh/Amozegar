using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassStateRepository : GenericRepository<ClassStates>, IClassStateRepository
    {
        public ClassStateRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<ClassStates?> GetClassStateByStateAsync(string state)
        {
            return await this._context.ClassesStates.SingleAsync(cs => cs.State == state);
        }
    }
}
