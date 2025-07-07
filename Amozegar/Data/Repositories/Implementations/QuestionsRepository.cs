using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class QuestionsRepository : GenericRepository<Question>, IQuestionsRepository
    {
        public QuestionsRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<Question> GetQuestionByExamIdByQuestionIndexAsync(int examId, int questionIndex)
        {
            var question = await this._context.Questions
                .Where(q => q.ExamId == examId)
                .OrderByDescending(q => q.QuestionId)
                .Skip(questionIndex - 1)
                .Take(1)
                .SingleAsync();

            return question;
        }

        public async Task<Question?> GetQuestionByExamIdByQuestionIdWithOptionsAsync(int examId, int questionId)
        {
            var question = await this._context.Questions
                .SingleOrDefaultAsync(q => q.QuestionId == questionId && q.ExamId == examId);
            return question;
        }

        public async Task<List<QuestionsAskAndOptionsCountViewModel>> GetQuestionsByExamIdForShowByPageNumberAsync(int examId, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var questionsAsks = await this._context.Questions
                .Where(q => q.ExamId == examId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new QuestionsAskAndOptionsCountViewModel()
                {
                    QuestionAsk = q.QuestionAsk,
                    OptionsCount = q.QuestionOptions.Count(),
                    QuestionId = q.QuestionId
                })
                .ToListAsync();
            return questionsAsks;
        }

        public async Task<int> GetQuestionsCountByExamIdForShowAsync(int examId)
        {
            var questionsAsksCount = await this._context.Questions
                .CountAsync(q => q.ExamId == examId);
            return questionsAsksCount;
        }
    }
}
