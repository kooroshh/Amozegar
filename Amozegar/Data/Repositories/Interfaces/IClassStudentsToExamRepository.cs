using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentsToExamRepository : IGenericRepository<ClassStudentsToExam>
    {
        Task<ClassStudentsToExam?> GetByClassStudentIdByExamIdAsync(int studentId, int examId);
        Task<IEnumerable<ClassStudentsToExam>> GetByExamIdAsync(int examId);
        Task<ClassStudentsToExam?> GetIsFinishedByExamIdByStudentIdAsync(int examId, int studentId);
        Task<int> ClassStudentsToExamByExamIdCountAsync(int examId);
        Task<List<StudentResult>> GetByExamIdForShowAsync(int examId, int pageNumber);
        Task<int> CountByExamIdForShowAsync(int examId);
    }
}
