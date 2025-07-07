using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IClassStudentToExamToQuestionRepository : IGenericRepository<ClassStudentsToExamToQuestion>
    {
        Task<ClassStudentsToExamToQuestion?> GetByClassStudentToExamIdByQuestionIdAsync(int classStudentToExamId, int questionId);
        Task<int> TrueAwnserCountsByClassStudentToExamIdCountAsync(int classStudentToExamId);
    }
}
