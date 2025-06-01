using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsToHomeworksRepository : IGenericRepository<ClassStudentsToHomework>
    {
        Task<ClassStudentsToHomework?> GetByHomeworkIdByClassStudentIdAsync(int homeworkId, int classStudentId);
        Task<int> GetCountByClassIdentityForSentListAsync(string classIdentity);
        Task<IEnumerable<HomeworksSentViewModel>> GetByClassIdentityByPageNumberForSentListAsync(string classIdentity, int pageNumber);
        Task<HomeworkSentCheckViewModel?> GetByClassIdByIdForCheckSentAsync(string classIdentity, int studentToHomeworkId);
        Task<bool> GetByClassIdentityByIdForChangeStateAsync(string classIdentity, int studentToHomeworkId);
        Task ChangeStateByClassIdentityByIdByStateAsync(string classIdentity, int studentToHomeworkId, string state);
    }
}
