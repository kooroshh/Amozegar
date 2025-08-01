using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Areas.Admin.Models.ExamsViewModel>> GetExamsByPagNumberAsync(int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var exams = await this._context.Exams
                .Where(e => e.ExamState.State != "Deleted")
                .OrderByDescending(h => h.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new Areas.Admin.Models.ExamsViewModel()
                {
                    ExamId = e.ExamId,
                    Title = e.ExamTitle,
                    QuestionCount = e.Questions.Count(),
                    CreatedAt = e.CreatedAt.ToShamsi(),
                    PersianState = e.ExamState.PersianState,
                    State = e.ExamState.State,
                    ClassIdentity = e.ClassRoam.ClassIdentity,
                })
                .ToListAsync();

            return exams;

        }

        public async Task<int> GetNotDeletedExamsCountAsync()
        {
            var count = await this._context.Exams
                .CountAsync(e => e.ExamState.State != "Deleted");

            return count;
        }

        public async Task<Areas.Admin.Models.ExamViewModel?> GetExamByIdForAdminByPageNumbersAsync(int examId, int studentsPageNumber, int questionsPageNumber)
        {
            int pageStudent = studentsPageNumber > 0 ? studentsPageNumber : 0;
            int pageSizeStudent = studentsPageNumber > 0 ? DefaultPageCount.Count : 0;

            int pageQuestion = questionsPageNumber > 0 ? questionsPageNumber : 0;
            int pageSizeQuestion = questionsPageNumber > 0 ? DefaultPageCount.Count : 0;

            var exam = await this._context.Exams
                .Where(e => e.ExamId == examId && e.ExamState.State != "Deleted")
                .Select(e => new Areas.Admin.Models.ExamViewModel()
                {
                    ExamId = e.ExamId,
                    ExamTitle = e.ExamTitle,
                    ExamDescription = e.ExamDescription,
                    EndAt = e.EndDate.ToShamsi(),
                    StartAt = e.StartDate.ToShamsi(),
                    CreatedAt = e.CreatedAt.ToShamsi(),
                    ClassIdentity = e.ClassRoam.ClassIdentity,
                    PersianState = e.ExamState.PersianState,
                    QuestionCount = e.Questions.Count(),
                    JoinerCount = e.ClassStudentsToExam.Count(),
                    ScoreAvarage = (int?)e.ClassStudentsToExam.Average(cste => (double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count() * 100) ?? 0,
                    AcceptedCount = e.ClassStudentsToExam.Count(cste => ((double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count() * 100) >= 50),
                })
                .SingleOrDefaultAsync();

            if (exam == null)
            {
                return null;
            }

            exam.Students = await this._context.ClassStudentsToExam
                .Where(cste => cste.ExamId == examId)
                .Skip((pageStudent - 1) * pageSizeStudent)
                .Take(pageSizeStudent)
                .Select(cste => new Areas.Admin.Models.StudentResultViewModel()
                {
                    JoindAt = cste.JoinAt.ToShamsi(),
                    ExamStatus = cste.IsFinish ? "تکمیل شده" : "تکمیل نشده",
                    Score = (int)Math.Round((double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count() * 100),
                    IncorrectAwnser = cste.ClassStudentsToExamToQuestions.Count(cstetq => !cstetq.IsTrueAwnser),
                    CorrectAwnser = cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser),
                    AwnserCount = cste.LastCompletedQuestion,
                    StudentName = cste.ClassStudent.User.FullName,
                    StudentPicturePath = cste.ClassStudent.User.PicturePath
                })
                .ToListAsync();

            exam.Questions = await this._context.Questions
                .Where(q => q.ExamId == examId)
                .Skip((pageQuestion - 1) * pageSizeQuestion)
                .Take(pageSizeQuestion)
                .Select(q => new Areas.Admin.Models.QuestionsViewModel()
                {
                    QuestionId = q.QuestionId,
                    OptionsCount = q.QuestionOptions.Count(),
                    Awnser = q.Answer,
                    Question = q.QuestionAsk,
                })
                .ToListAsync();



            return exam;
        }

        public async Task<ExamRsultsViewModel?> GetExamResultsByIdByClassIdentityAsync(int pageNumber, string classIdentity, int examId)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);


            var exam = await this._context.Exams
                .Where(e => e.ClassId == clsId && e.ExamId == examId && e.ExamState.State == "Completed")
                .Select(e => new ExamRsultsViewModel()
                {
                    ExamTitle = e.ExamTitle,
                    ExamDescription = e.ExamDescription,
                    ExamId = e.ExamId,
                    CreatedAt = e.CreatedAt.ToShamsi(),
                    EndDate = e.EndDate.ToShamsi(),
                    StartDate = e.StartDate.ToShamsi(),
                    State = e.ExamState.PersianState,
                    QuestionCount = e.Questions.Count(),
                    Joiner = e.ClassStudentsToExam.Count(),
                    Accepted = e.ClassStudentsToExam.Count(cste => ((double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count() * 100) >= 50),
                    ScoreAvarage = (int?)e.ClassStudentsToExam.Average(cste => (double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count() * 100) ?? 0,
                    StudentsResults = new()
                })
                .SingleOrDefaultAsync();

            if (exam == null)
            {
                return null;
            }



            exam.StudentsResults = await this._context.ClassStudentsToExam
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
                    Score = (int)Math.Round((double)cste.ClassStudentsToExamToQuestions.Count(cstetq => cstetq.IsTrueAwnser) / cste.Exam.Questions.Count() * 100),
                    StudentStatus = cste.IsFinish ? "تکمیل شده" : "تکمیل نشده",
                    FalseAwnserCount = cste.ClassStudentsToExamToQuestions.Count(cstetq => !cstetq.IsTrueAwnser)
                })
                .ToListAsync();


            return exam;
        }

        public async Task<Exam?> GetExamByIdByThisStatesAsync(int id, params string[] states)
        {
            var exam = await this._context.Exams
                .Where(e => e.ExamId == id && states.Contains(e.ExamState.State))
                .SingleOrDefaultAsync();


            return exam;
        }

        public async Task ChangeExamStateAsync(Exam exam, string to)
        {
            var state = await this._context.ExamStates
                .SingleAsync(es => es.State == to);


            exam.ExamState = state;
            exam.ExamStateId = state.ExamStateId;

        }
    }
}
