using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IHomeworkRepository : IGenericRepository<Homework>
    {
        Task<IEnumerable<HomeworksViewModel>> GetHomeworksByClassIdentityByStudentIdByPageNumberAsync(string classIdentity, string studentId , int pageNumber);
        Task<int> GetHomeworksCountByClassIdentityAsync(string classIdentity);
        Task<ChangeHomeworkStateViewModel?> GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(string classIdentity, int homeworkId, string state);
        Task<ChangeHomeworkStateViewModel?> GetHomeworkByClassIdentityByHomeworkIdByNotThisStateForChangeStateAsync(string classIdentity, int homeworkId, string state);
        Task ChangeHomeworkState(int homeworkId, string state);
        Task<Homework?> GetHomeworkByClassIdentityByIdByNotThisStateAsync(string classIdentity, int homeworkId, string state);
        Task<HomeworkDetailsViewModel?> GetHomeworkWithPicturesByIdAndClassIdentityByStudentIdByIdByNotThisStateAsync(string classIdentity, string studentId, int homeworkId, string state);
        Task<ChangeHomeworkStateViewModel?> IsHomeworkExistByClassIdentityByIdByStateAsync(string classIdentity, int homeworkId, string state);
    }
}
