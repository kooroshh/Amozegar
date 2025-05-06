using Amozegar.Areas.Panel.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassesRepository : IGenericRepository<ClassRoam>
    {
        Task<IEnumerable<ClassesViewModel>> GetStudentsClasses(User user);
        Task<IEnumerable<ClassesViewModel>> GetTeachersClasses(User user);
        Task<ClassRoam?> GetByCheckStudentIsInClass(string studentName, int classId);
        Task<ClassRoam?> GetActiveClassByIdentity(string classIdentity);
        Task<List<AddStudentViewModel>?> GetClassStudentsRequests(string classIdentity);
        Task<int> GetClassStudentsRequestsCountAsync(string classIdentity);
        Task<ClassRoam?> GetClassByIdAndState(int classId, string teacherName, string state);
        Task<ClassRoam?> GetByClassIdentity(string classIdentity);
    }
}
