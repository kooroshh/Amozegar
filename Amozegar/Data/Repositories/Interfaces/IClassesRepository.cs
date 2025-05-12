using Amozegar.Areas.Panel.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassesRepository : IGenericRepository<ClassRoam>
    {
        Task<IEnumerable<ClassesViewModel>> GetStudentsClassesByUserByPageNumberAsync(User user, int pageNumber);
        Task<int> GetStudentsClassesCountByPageNumberAsync(User user);
        Task<IEnumerable<ClassesViewModel>> GetTeachersClassesByUserByPageNumberAsync(User user, int pageNumber);
        Task<int> GetTeachersClassesCountByPageNumberAsync(User user);
        Task<ClassRoam?> GetByCheckStudentIsInClassAsync(string studentName, int classId);
        Task<ClassRoam?> GetActiveClassByIdentityAsync(string classIdentity);
        Task<ClassRoam?> GetClassByIdAndStateAsync(int classId, string teacherName, string state);
        Task<ClassRoam?> GetByClassIdentityAsync(string classIdentity);
        Task<bool> IsStudentInClassByClassIdentityAndUserIdAsync(string classIdentity, string userId);
        Task<bool> IsClassForTeacherByClassIdentityAndUserIdAsync(string classIdentity, string userId);
    }
}
