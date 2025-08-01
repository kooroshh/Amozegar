using Amozegar.Data.Repositories;
using Amozegar.Data.Repositories.Implementations;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;

namespace Amozegar.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Report> ReportRepository { get; }
        public IClassesRepository ClassesRepository { get; }
        public IClassStudentsRepository ClassStudentsRepository { get; }
        public IClassStudentsStatesRepository ClassStudentsStatesRepository { get; }
        public IClassStateRepository ClassStateRepository { get; }
        public IGenericRepository<TableType> TableTypesRepository { get; }
        public IPicturesRepository PictureRepository { get; }
        public INotificationsRepository NotificationsRepository { get; }
        public IUsersViewsRepository UsersViewsRepository { get; }
        public IHomeworkStateRepository HomeworkStateRepository { get; }
        public IClassStudentsToHomeworksStatesRepository ClassStudentsToHomeworksStatesRepository { get; }
        public IHomeworkRepository HomeworkRepository { get; }
        public IClassStudentsToHomeworksRepository ClassStudentsToHomeworksRepository { get; }
        public IExamStateRepository ExamStatesRepository { get; }
        public IExamRepository ExamRepository { get; }
        public IQuestionsRepository QuestionsRepository { get; }
        public IQuestionOptionsRepository QuestionOptionsRepository { get; }
        public IClassStudentsToExamRepository ClassStudentsToExamRepository { get; }
        public IClassStudentToExamToQuestionRepository ClassStudentToExamToQuestionRepository { get; }
        public IUserRepository UsersRepository { get; }
        public IRolesRepository RolesRepository { get; }
        public ITicketsRepository TicketsRepository { get; }
        public IDashboardRepository DashboardRepository { get; }
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
