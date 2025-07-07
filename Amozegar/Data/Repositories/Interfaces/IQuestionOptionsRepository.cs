using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IQuestionOptionsRepository : IGenericRepository<QuestionOption>
    {
        Task<List<QuestionOption>> GetOptionsByQuestionIdByPageNumberAsync(int questionId, int pageNumber);
        Task<List<QuestionOption>> GetOptionsByQuestionIdAsync(int questionId);
        Task<bool> ExistByQuestionIdByOptionsAsync(int questionId, List<string> option);
        Task<int> GetCountOptionsByQuestionIdAsync(int questionId);
        Task<QuestionOption?> GetByQuestionIdByOptionIdAsync(int questionId, int optionId);
        Task<List<QuestionOptionForShowViewModel>> GetOptionsByQuestionIdByPageNumberByAwnserIdAsync(int questionId, int pageNumber, int awnserId = 0);
    }
}
