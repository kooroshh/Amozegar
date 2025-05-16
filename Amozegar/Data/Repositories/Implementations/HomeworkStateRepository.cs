using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class HomeworkStateRepository : GenericRepository<HomeworkState>, IHomeworkStateRepository
    {
        public HomeworkStateRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<HomeworkState> GetHomeworkStateByStateAsync(string state)
        {
            var homeworkState = await this._context.HomeworksStates
                .SingleAsync(hs => hs.State == state);

            return homeworkState;
        }
    }
}
