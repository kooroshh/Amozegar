using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IExamStateRepository : IGenericRepository<ExamState>
    {
        Task<ExamState> GetByStateAsync(string state);
        Task<IEnumerable<ExamState>> GetStatesByStates(params string[] states);
    }
}
