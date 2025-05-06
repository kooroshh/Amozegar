using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsRepository : IGenericRepository<ClassStudents>
    {
        Task<ClassStudents?> GetByCheckStudentIsInClass(User student , int classId);
        Task<ClassStudents?> GetStudentInClassByClassIdentityAndClassStudentIdAsync(int studentInClassId , string classId);
    }
}
