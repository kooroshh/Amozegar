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

        public async Task<int> ClassStudentsToExamByExamIdCountAsync(int examId)
        {
            var classStudentToExam = await this._context.ClassStudentsToExam
                .CountAsync(cste => cste.ExamId == examId);

            return classStudentToExam;
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

        public async Task<List<StudentResult>> GetByExamIdForShowAsync(int examId, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var studentResults = await this._context.ClassStudentsToExam
                .Where(cste => cste.ExamId == examId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(cste => new StudentResult()
                {
                    StudentImage = cste.ClassStudent.User.PicturePath,
                    StudentName = cste.ClassStudent.User.FullName,
                    JoinAt = cste.JoinAt.ToShamsi(),
                    AwnserCount = cste.LastCompletedQuestion,
                    TrueAwnserCount = cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser),
                    Score = (int)Math.Round(((double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count()) * 100),
                    StudentStatus = cste.IsFinish ? "تکمیل شده" : "تکمیل نشده",
                    FalseAwnserCount = cste.ClassStudentsToExamToQuestions.Count(cstetq => !cstetq.IsTrueAwnser)
                })
                .ToListAsync();

            return studentResults;
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
