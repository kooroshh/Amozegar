using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsToHomeworksStatesRepository : IGenericRepository<ClassStudentsToHomeworkState>
    {
        Task<ClassStudentsToHomeworkState> GetClassStudentsToHomeworksStateByStateAsync(string state);
    }
}
