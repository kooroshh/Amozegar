using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsRepository : IGenericRepository<ClassStudents>
    {
        Task<ClassStudents?> GetByCheckStudentIsInClassAsync(User student , int classId);
        Task<ClassStudents?> GetStudentInClassByClassIdentityAndClassStudentIdAsync(int studentInClassId , string classId);
        Task<IEnumerable<StudentsListViewModel>> GetStudentsByClassIdentityByStateAsync(string classIdentity, string state);
        Task<IEnumerable<StudentsListForStudentsViewModel>> GetStudentsByClassIdentityByStateForStudentsAsync(string classIdentity, string state);
    }
}
