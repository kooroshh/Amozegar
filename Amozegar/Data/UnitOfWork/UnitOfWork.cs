
using Amozegar.Data.Repositories;
using Amozegar.Data.Repositories.Implementations;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;

namespace Amozegar.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private AmozegarContext _context;
        private UserManager<User> _userManager;
        private IGenericRepository<Report> _reportRepository;
        private IClassesRepository _classesRepository;
        private IClassStudentsRepository _classStudentsRepository;
        private IClassStudentsStatesRepository _classStudentsStatesRepository;
        private IClassStateRepository _classStateRepository;
        private IGenericRepository<TableType> _pictureTypesRepository;
        private IPicturesRepository _picturesRepository;
        private INotificationsRepository _notificationRepository;
        private IUsersViewsRepository _usersViewsRepository;
        private IHomeworkStateRepository _homeworkStateRepository;
        private IClassStudentsToHomeworksStatesRepository _classStudentsToHomeworksStatesRepository;
        private IHomeworkRepository _homeworkRepository;
        private IClassStudentsToHomeworksRepository _classStudentsToHomeworksRepository;
        private IExamStateRepository _examStatesRepository;
        private IExamRepository _examRepository;
        private IQuestionsRepository _questionsRepository;
        private IQuestionOptionsRepository _questionOptionsRepository;
        private IClassStudentsToExamRepository _classStudentsToExamRepository;
        private IClassStudentToExamToQuestionRepository _classStudentToExamToQuestionRepository;
        private IUserRepository _userRepository;
        private IRolesRepository _rolesRepository;
        private ITicketsRepository _ticketsRepository;
        private IDashboardRepository _dashboardRepository;

        public UnitOfWork(AmozegarContext context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public IGenericRepository<Report> ReportRepository 
        {
            get
            {
                if (this._reportRepository == null)
                {
                    this._reportRepository = new GenericRepository<Report>(this._context);
                }
                return this._reportRepository;
            }
        }

        public IClassesRepository ClassesRepository
        {
            get
            {
                if (this._classesRepository == null)
                {
                    this._classesRepository = new ClassesRepository(this._context, this._userManager);
                }
                return this._classesRepository;
            }
        }

        public IClassStudentsRepository ClassStudentsRepository
        {
            get
            {
                if (this._classStudentsRepository == null)
                {
                    this._classStudentsRepository = new ClassStudentsRepository(this._context, this._userManager);
                }
                return this._classStudentsRepository;
            }
        }

        public IClassStudentsStatesRepository ClassStudentsStatesRepository
        {
            get
            {
                if (this._classStudentsStatesRepository == null)
                {
                    this._classStudentsStatesRepository = new ClassStudentsStatesRepository(this._context);
                }
                return this._classStudentsStatesRepository;
            }
        }

        public IClassStateRepository ClassStateRepository
        {
            get
            {
                if (this._classStateRepository == null)
                {
                    this._classStateRepository = new ClassStateRepository(this._context);
                }
                return this._classStateRepository;
            }
        }

        public IGenericRepository<TableType> TableTypesRepository
        {
            get
            {
                if (this._pictureTypesRepository == null)
                {
                    this._pictureTypesRepository = new GenericRepository<TableType>(this._context);
                }
                return this._pictureTypesRepository;
            }
        }

        public INotificationsRepository NotificationsRepository
        {
            get
            {
                if (this._notificationRepository == null)
                {
                    this._notificationRepository = new NotificationsRepository(this._context);
                }
                return this._notificationRepository;
            }
        }

        public IPicturesRepository PictureRepository
        {
            get
            {
                if (this._picturesRepository == null)
                {
                    this._picturesRepository = new PicturesRepository(this._context);
                }
                return this._picturesRepository;
            }
        }

        public IUsersViewsRepository UsersViewsRepository
        {
            get
            {
                if (this._usersViewsRepository == null)
                {
                    this._usersViewsRepository = new UsersViewsRepository(this._context);
                }
                return this._usersViewsRepository;
            }
        }

        public IHomeworkStateRepository HomeworkStateRepository
        {
            get
            {
                if (this._homeworkStateRepository == null)
                {
                    this._homeworkStateRepository = new HomeworkStateRepository(this._context);
                }
                return this._homeworkStateRepository;
            }
        }

        public IClassStudentsToHomeworksStatesRepository ClassStudentsToHomeworksStatesRepository
        {
            get
            {
                if (this._classStudentsToHomeworksStatesRepository == null)
                {
                    this._classStudentsToHomeworksStatesRepository = new ClassStudentsToHomeworksStatesRepository(this._context);
                }
                return this._classStudentsToHomeworksStatesRepository;
            }
        }

        public IHomeworkRepository HomeworkRepository
        {
            get
            {
                if (this._homeworkRepository == null)
                {
                    this._homeworkRepository = new HomeworkRepository(this._context);
                }
                return this._homeworkRepository;
            }
        }

        public IClassStudentsToHomeworksRepository ClassStudentsToHomeworksRepository
        {
            get
            {
                if (this._classStudentsToHomeworksRepository == null)
                {
                    this._classStudentsToHomeworksRepository = new ClassStudentsToHomeworksRepository(this._context, this._userManager);
                }
                return this._classStudentsToHomeworksRepository;
            }
        }

        public IExamStateRepository ExamStatesRepository
        {
            get
            {
                if (this._examStatesRepository == null)
                {
                    this._examStatesRepository = new ExamStateRepository(this._context);
                }
                return this._examStatesRepository;
            }
        }

        public IExamRepository ExamRepository
        {
            get
            {
                if (this._examRepository == null)
                {
                    this._examRepository = new ExamRepository(this._context);
                }
                return this._examRepository;
            }
        }

        public IQuestionsRepository QuestionsRepository
        {
            get
            {
                if (this._questionsRepository == null)
                {
                    this._questionsRepository = new QuestionsRepository(this._context);
                }
                return this._questionsRepository;
            }
        }

        public IQuestionOptionsRepository QuestionOptionsRepository
        {
            get
            {
                if (this._questionOptionsRepository == null)
                {
                    this._questionOptionsRepository = new QuestionOptionsRepository(this._context);
                }
                return this._questionOptionsRepository;
            }
        }

        public IClassStudentsToExamRepository ClassStudentsToExamRepository
        {
            get
            {
                if (this._classStudentsToExamRepository == null)
                {
                    this._classStudentsToExamRepository = new ClassStudentsToExamRepository(this._context);
                }
                return this._classStudentsToExamRepository;
            }
        }

        public IClassStudentToExamToQuestionRepository ClassStudentToExamToQuestionRepository
        {
            get
            {
                if (this._classStudentToExamToQuestionRepository == null)
                {
                    this._classStudentToExamToQuestionRepository = new ClassStudentToExamToQuestionRepository(this._context);
                }
                return this._classStudentToExamToQuestionRepository;
            }
        }

        public IUserRepository UsersRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(this._context, _userManager);
                }
                return this._userRepository;
            }
        }

        public IRolesRepository RolesRepository
        {
            get
            {
                if (this._rolesRepository == null)
                {
                    this._rolesRepository = new RolesRepository(this._context);
                }
                return this._rolesRepository;
            }
        }

        public ITicketsRepository TicketsRepository
        {
            get
            {
                if (this._ticketsRepository == null)
                {
                    this._ticketsRepository = new TicketsRepository(this._context);
                }
                return this._ticketsRepository;
            }
        }

        public IDashboardRepository DashboardRepository
        {
            get
            {
                if (this._dashboardRepository == null)
                {
                    this._dashboardRepository = new DashboardRepository(this._context);
                }
                return this._dashboardRepository;
            }
        }

        public void Dispose()
        {
            this._context.Dispose();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await this._context.SaveChangesAsync(cancellationToken);
        }
    }
}
