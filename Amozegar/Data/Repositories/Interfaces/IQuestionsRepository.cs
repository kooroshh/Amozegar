using Amozegar.Areas.Admin.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IQuestionsRepository : IGenericRepository<Question>
    {
        Task<List<QuestionsAskAndOptionsCountViewModel>> GetQuestionsByExamIdForShowByPageNumberAsync(int examId, int pageNumber);
        Task<int> GetQuestionsCountByExamIdForShowAsync(int examId);
        Task<Question?> GetQuestionByExamIdByQuestionIdWithOptionsAsync(int examId, int questionId);
        Task<Question> GetQuestionByExamIdByQuestionIndexAsync(int examId, int questionIndex);
        Task<QuestionViewModel?> GetQuestionByIdByPageNumberForAdminAsync(int questionId, int pageNumber);
    }
}
