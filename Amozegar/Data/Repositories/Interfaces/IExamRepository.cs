using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IExamRepository : IGenericRepository<Exam>
    {
        Task<IEnumerable<ExamsViewModel>> GetByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber);
        Task<int> GetCountByClassIdentityAsync(string classIdentity);
        Task<int> GetCountByClassIdentityByStatesAsync(string classIdentity, params string[] states);
        Task<IEnumerable<ExamsViewModel>> GetByClassIdentityByPageNumberByStatesAsync(string classIdentity, int pageNumber,params string[] states);
        Task<Exam?> GetByClassIdentityByExamIdAsync(string classIdentity, int examId);
        Task ChangeStateByExamAsync(Exam exam, string toState);
        Task<ExamViewModel?> GetExamByClassIdByExamIdForDetailsAsync(string classIdentity, int examId);
        Task<bool> IsExistByExamIdByClassIdentityForOngoingAsync(string classIdentity, int examId);
        Task<DateTime> GetExamEndDateByClassIdentityByExamIdAsync(string classIdentity, int examId);
        Task<List<Exam>> GetForBackgroundAsync(CancellationToken stoppingToken = default);
        Task<IEnumerable<Areas.Admin.Models.ExamsViewModel>> GetExamsByPagNumberAsync(int pageNumber);
        Task<int> GetNotDeletedExamsCountAsync();
        Task<Areas.Admin.Models.ExamViewModel?> GetExamByIdForAdminByPageNumbersAsync(int examId, int studentsPageNumber, int questionsPageNumber);
        Task<ExamRsultsViewModel?> GetExamResultsByIdByClassIdentityAsync(int pageNumber, string classIdentity, int examId);
        Task<Exam?> GetExamByIdByThisStatesAsync(int id, params string[] states);
        Task ChangeExamStateAsync(Exam exam, string to);
    }
}
