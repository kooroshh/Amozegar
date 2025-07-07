using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassStudentToExamToQuestionRepository : GenericRepository<ClassStudentsToExamToQuestion>, IClassStudentToExamToQuestionRepository
    {
        public ClassStudentToExamToQuestionRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<ClassStudentsToExamToQuestion?> GetByClassStudentToExamIdByQuestionIdAsync(int classStudentToExamId, int questionId)
        {
            var classStudentToExamQuestion = await this._context.ClassStudentsToExamsToQuestions
                .SingleOrDefaultAsync(cstetq => cstetq.ClassStudentToExamId == classStudentToExamId && cstetq.QuestionId == questionId);

            return classStudentToExamQuestion;
        }

        public async Task<int> TrueAwnserCountsByClassStudentToExamIdCountAsync(int classStudentToExamId)
        {
            var count = await this._context.ClassStudentsToExamsToQuestions
                .CountAsync(cstetq => cstetq.ClassStudentToExamId == classStudentToExamId && cstetq.IsTrueAwnser == true);

            return count;
        }
    }
}
