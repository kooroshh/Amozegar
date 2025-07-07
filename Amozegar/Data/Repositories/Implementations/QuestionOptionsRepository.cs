using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class QuestionOptionsRepository : GenericRepository<QuestionOption>, IQuestionOptionsRepository
    {
        public QuestionOptionsRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<bool> ExistByQuestionIdByOptionsAsync(int questionId, List<string> options)
        {
            var isExist = await this._context.QuestionOptions
                .AnyAsync(qo => qo.QuestionId == questionId && options.Contains(qo.Option));

            return isExist;
        }

        public async Task<QuestionOption?> GetByQuestionIdByOptionIdAsync(int questionId, int optionId)
        {
            var option = await this._context.QuestionOptions
                .SingleOrDefaultAsync(qo => qo.QuestionId == questionId && qo.QuestionOptionId == optionId);

            return option;
        }

        public async Task<int> GetCountOptionsByQuestionIdAsync(int questionId)
        {
            var questionOptions = await this._context.QuestionOptions
                .CountAsync(qo => qo.QuestionId == questionId);
            return questionOptions;
        }

        public async Task<List<QuestionOption>> GetOptionsByQuestionIdAsync(int questionId)
        {
            var questionOptions = await this._context.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .ToListAsync();
            return questionOptions;
        }

        public async Task<List<QuestionOption>> GetOptionsByQuestionIdByPageNumberAsync(int questionId, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var questionOptions = await this._context.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return questionOptions;
        }

        public async Task<List<QuestionOptionForShowViewModel>> GetOptionsByQuestionIdByPageNumberByAwnserIdAsync(int questionId, int pageNumber, int awnserId = 0)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var questionOptions = await this._context.QuestionOptions
                .Where(qo => qo.QuestionId == questionId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(qo => new QuestionOptionForShowViewModel()
                {
                    Option = qo.Option,
                    QuestionOptionId = qo.QuestionOptionId,
                    IsAwnser = qo.QuestionOptionId == awnserId
                })
                .ToListAsync();
            return questionOptions;
        }
    }
}
