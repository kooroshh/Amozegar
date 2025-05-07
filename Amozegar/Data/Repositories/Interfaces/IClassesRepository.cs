using Amozegar.Areas.Panel.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassesRepository : IGenericRepository<ClassRoam>
    {
        Task<IEnumerable<ClassesViewModel>> GetStudentsClassesAsync(User user);
        Task<IEnumerable<ClassesViewModel>> GetTeachersClassesAsync(User user);
        Task<ClassRoam?> GetByCheckStudentIsInClassAsync(string studentName, int classId);
        Task<ClassRoam?> GetActiveClassByIdentityAsync(string classIdentity);
        Task<List<AddStudentViewModel>?> GetClassStudentsRequestsAsync(string classIdentity);
        Task<int> GetClassStudentsRequestsCountAsync(string classIdentity);
        Task<ClassRoam?> GetClassByIdAndStateAsync(int classId, string teacherName, string state);
        Task<ClassRoam?> GetByClassIdentityAsync(string classIdentity);
    }
}
