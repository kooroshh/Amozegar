using Amozegar.Data.Repositories.Implementations;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IStudentsHomeworksRepository : IGenericRepository<ClassStudentsToHomework>
    {
        Task AddStudentToHomeworkByHomeworkIdAsync(int homeworkId, int classStudentId);
    }
}
