using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStateRepository : IGenericRepository<ClassStates>
    {
        Task<ClassStates?> GetClassStateByStateAsync(string state);
    }
}
