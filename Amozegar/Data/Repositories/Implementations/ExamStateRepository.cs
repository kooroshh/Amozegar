using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ExamStateRepository : GenericRepository<ExamState>, IExamStateRepository
    {
        public ExamStateRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<ExamState> GetByStateAsync(string state)
        {
            var examState = await this._context.ExamStates.SingleAsync(es => es.State == state);
            return examState;
        }

        public async Task<IEnumerable<ExamState>> GetStatesByStates(params string[] states)
        {
            List<ExamState> exmaStates = new List<ExamState>();

            foreach (var state in states)
            {
                exmaStates.Add(await this.GetByStateAsync(state));
            }

            return exmaStates;
        }
    }
}
