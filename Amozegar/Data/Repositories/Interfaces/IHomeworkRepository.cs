using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IHomeworkRepository : IGenericRepository<Homework>
    {
        Task<IEnumerable<HomeworksViewModel>> GetHomeworskByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber);
        Task<int> GetHomeworksCountByClassIdentityAsync(string classIdentity);
        Task<ChangeHomeworkStateViewModel?> GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(string classIdentity, int homeworkId, string state);
        Task<ChangeHomeworkStateViewModel?> GetHomeworkByClassIdentityByHomeworkIdByNotThisStateForChangeStateAsync(string classIdentity, int homeworkId, string state);
        Task ChangeHomeworkState(int homeworkId, string state);
        Task<Homework?> GetHomeworkByIdByNotThisStateAsync(string classIdentity, int homeworkId, string state);
    }
}
