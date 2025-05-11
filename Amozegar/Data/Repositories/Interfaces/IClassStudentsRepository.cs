using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsRepository : IGenericRepository<ClassStudents>
    {
        Task<ClassStudents?> GetByCheckStudentIsInClassAsync(User student , int classId);
        Task<ClassStudents?> GetStudentInClassByClassIdentityAndClassStudentIdAsync(int studentInClassId , string classId);
        Task<IEnumerable<StudentsListViewModel>> GetStudentsByClassIdentityByStateByPageNumberAsync(string classIdentity, string state, int pageNumber);
        Task<int> GetStudentsCountByClassIdentityByStateAsync(string classIdentity, string state);
        Task<int> GetClassStudentsRequestsCountAsync(string classIdentity);
        Task<List<AddStudentViewModel>?> GetClassStudentsRequestsByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber);
    }
}
