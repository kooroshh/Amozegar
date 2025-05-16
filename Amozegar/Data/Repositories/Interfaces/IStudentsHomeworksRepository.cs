using Amozegar.Data.Repositories.Implementations;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IStudentsHomeworksRepository : IGenericRepository<StudentHomework>
    {
        Task AddStudentToHomeworkByHomeworkIdAsync(int homeworkId, int classStudentId);
    }
}
