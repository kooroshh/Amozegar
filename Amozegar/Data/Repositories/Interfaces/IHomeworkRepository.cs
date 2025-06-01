using Amozegar.Areas.Shared.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IHomeworkRepository : IGenericRepository<Homework>
    {
        Task<IEnumerable<HomeworksViewModel>> GetHomeworksByClassIdentityByStudentIdByPageNumberAsync(string classIdentity, string studentId , int pageNumber);
        Task<IEnumerable<HomeworksViewModel>> GetNotSentHomeworksByClassIdentityByStudentIdByPageNumber(string classIdentity, string studentId, int pageNumber);
        Task<int> GetHomeworksCountByClassIdentityAsync(string classIdentity);
        Task<int> GetNotSentHomeworksCountByClassIdentityByStudentIdAsync(string classIdentity, string studentId);
        Task<ChangeHomeworkViewModel?> GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(string classIdentity, int homeworkId, string state);
        Task ChangeHomeworkState(int homeworkId, string state);
        Task<Homework?> GetHomeworkByClassIdentityByIdByNotThisStateAsync(string classIdentity, int homeworkId, string state);
        Task<HomeworkDetailsViewModel?> GetHomeworkWithPicturesByIdAndClassIdentityByStudentIdByIdByNotThisStateAsync(string classIdentity, string studentId, int homeworkId, string state);
        Task<ChangeHomeworkViewModel?> IsHomeworkExistByClassIdentityByIdByStateAsync(string classIdentity, int homeworkId, string state);
    }
}
