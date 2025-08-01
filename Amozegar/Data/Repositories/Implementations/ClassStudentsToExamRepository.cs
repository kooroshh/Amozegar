using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassStudentsToExamRepository : GenericRepository<ClassStudentsToExam>, IClassStudentsToExamRepository
    {
        public ClassStudentsToExamRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<int> CountByExamIdForShowAsync(int examId)
        {
            var studentResults = await this._context.ClassStudentsToExam
                .CountAsync(cste => cste.ExamId == examId);

            return studentResults;
        }

        public async Task<ClassStudentsToExam?> GetByClassStudentIdByExamIdAsync(int studentId, int examId)
        {
            var classStudentToExam = await this._context.ClassStudentsToExam
                .SingleOrDefaultAsync(cste => cste.ClassStudentId == studentId && cste.ExamId == examId);

            return classStudentToExam;
        }

        public async Task<IEnumerable<ClassStudentsToExam>> GetByExamIdAsync(int examId)
        {
            var classStudentToExam = await this._context.ClassStudentsToExam
                .Include(cste => cste.ClassStudentsToExamToQuestions)
                .Where(cste => cste.ExamId == examId)
                .ToListAsync();

            return classStudentToExam;
        }


        public async Task<ClassStudentsToExam?> GetIsFinishedByExamIdByStudentIdAsync(int examId, int studentId)
        {
            var classStudentToExam = await this._context.ClassStudentsToExam
                .Where(cste => cste.ClassStudentId == studentId && cste.ExamId == examId && cste.IsFinish == true)
                .SingleOrDefaultAsync();

            return classStudentToExam;
        }
    }
}
