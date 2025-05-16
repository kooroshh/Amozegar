using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IHomeworkStateRepository : IGenericRepository<HomeworkState>
    {
        Task<HomeworkState> GetHomeworkStateByStateAsync(string state);
    }
}
