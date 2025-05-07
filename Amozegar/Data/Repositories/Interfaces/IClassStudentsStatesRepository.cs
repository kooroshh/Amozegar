using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsStatesRepository : IGenericRepository<ClassStudentState>
    {
        Task<ClassStudentState> GetStateByNameAsync(string state);
    }
}
