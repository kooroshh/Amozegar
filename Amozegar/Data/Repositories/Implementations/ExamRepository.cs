using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Student.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        public ExamRepository(AmozegarContext context) : base(context)
        {
        }

        // Utilities

        private async Task<int> getClassIdByClassIdentityAsync(string classIdentity)
        {
            var cls = await this._context.Classes
                .Select(c => new { c.ClassIdentity, c.ClassId })
                .SingleAsync(c => c.ClassIdentity == classIdentity);

            return cls.ClassId;
        }

        private async Task<ExamState> getExamStateByState(string state)
        {
            var examState = await this._context.ExamStates
                .SingleAsync(s => s.State == state);
            return examState;
        }

        // Main Methods
        public async Task<IEnumerable<ExamsViewModel>> GetByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;


            var exams = await this._context.Exams
                .Include(e => e.ExamState)
                .Where(e => e.ClassId == clsId && e.ExamState.State != "Deleted")
                .OrderByDescending(h => h.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExamsViewModel()
                {
                    CreatedAt = e.CreatedAt.ToShamsi(),
                    ExamTitle = e.ExamTitle,
                    ExamId = e.ExamId,
                    PersianState = e.ExamState.PersianState,
                    EndAt = e.EndDate.ToShamsi(),
                    StartAt = e.StartDate.ToShamsi(),
                    QuestionsCount = e.Questions.Count(),
                    State = e.ExamState.State,
                })
                .ToListAsync();


            return exams;
        }

        public async Task<IEnumerable<ExamsViewModel>> GetByClassIdentityByPageNumberByStatesAsync(string classIdentity, int pageNumber,params string[] states)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            List<ExamState> examState = new List<ExamState>();
            
            foreach(var state in states)
            {
                examState.Add(await this.getExamStateByState(state));
            }

            var exams = await this._context.Exams
                .Include(e => e.ExamState)
                .Where(e => e.ClassId == clsId && examState.Contains(e.ExamState))
                .OrderByDescending(h => h.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExamsViewModel()
                {
                    CreatedAt = e.CreatedAt.ToShamsi(),
                    ExamTitle = e.ExamTitle,
                    ExamId = e.ExamId,
                    PersianState = e.ExamState.PersianState,
                    EndAt = e.EndDate.ToShamsi(),
                    StartAt = e.StartDate.ToShamsi(),
                    QuestionsCount = e.Questions.Count(),
                    State = e.ExamState.State,
                })
                .ToListAsync();


            return exams;
        }

        public async Task<int> GetCountByClassIdentityAsync(string classIdentity)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);

            var examsCount = await this._context.Exams
                .CountAsync(e => e.ClassId == clsId && e.ExamState.State != "Deleted");

            return examsCount;
        }

        public async Task<int> GetCountByClassIdentityByStatesAsync(string classIdentity, params string[] states)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);
            List<ExamState> examState = new List<ExamState>();

            foreach (var state in states)
            {
                examState.Add(await this.getExamStateByState(state));
            }

            var examsCount = await this._context.Exams
                .CountAsync(e => e.ClassId == clsId && examState.Contains(e.ExamState));

            return examsCount;
        }

        public async Task<Exam?> GetByClassIdentityByExamIdAsync(string classIdentity, int examId)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity); 

            var exam = await this._context.Exams
                .Include(e => e.ExamState)
                .SingleOrDefaultAsync(e => e.ClassId == clsId && e.ExamId == examId && e.ExamState.State != "Deleted");


            return exam;
        }

        public async Task ChangeStateByExamAsync(Exam exam, string toState)
        {
            var state = await this.getExamStateByState(toState);

            exam.ExamState = state;
            exam.ExamStateId = state.ExamStateId;

        }

        public async Task<ExamViewModel?> GetExamByClassIdByExamIdForDetailsAsync(string classIdentity, int examId)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);

            var exam = await this._context.Exams
                .Include(e => e.ExamState)
                .Where(e => e.ClassId == clsId && e.ExamId == examId && e.ExamState.State != "Deleted")
                .Select(e => new ExamViewModel()
                {
                    CreatedAt = e.CreatedAt.ToShamsi(),
                    EndDate = e.EndDate.ToShamsi(),
                    ExamDescription = e.ExamDescription,
                    ExamId = e.ExamId,
                    ExamTitle = e.ExamTitle,
                    StartDate = e.StartDate.ToShamsi(),
                    QuestionCount = e.Questions.Count(),
                    State = e.ExamState,
                })
                .SingleOrDefaultAsync();


            return exam;
        }

        public async Task<bool> IsExistByExamIdByClassIdentityForOngoingAsync(string classIdentity, int examId)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);

            var existExam = await this._context.Exams
                .AnyAsync(e =>
                    e.ExamId == examId &&
                    e.ClassId == clsId &&
                    e.ExamState.State == "Ongoing"
                );

            return existExam;
        }

        public async Task<DateTime> GetExamEndDateByClassIdentityByExamIdAsync(string classIdentity, int examId)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);

            var examEndDate = await this._context.Exams
                .Where(e => e.ClassId == clsId && e.ExamId == examId && e.ExamState.State != "Deleted")
                .Select(e => e.EndDate)
                .SingleOrDefaultAsync();


            return examEndDate;
        }

        public Task<List<Exam>> GetForBackgroundAsync(CancellationToken stoppingToken = default)
        {
            var exams = this._context.Exams
                .Include(e => e.ExamState)
                .Where(e =>
                            e.ExamState.State == "Ongoing" ||
                            e.ExamState.State == "Scheduled" &&
                            e.StartDate < DateTime.Now ||
                            e.EndDate < DateTime.Now
                            )
                .ToListAsync(stoppingToken);

            return exams;
        }
    }
}
