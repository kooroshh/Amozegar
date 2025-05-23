
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

        public void Dispose()
        {
            this._context.Dispose();
        }

        public async Task SaveChangesAsync()
        {
            await this._context.SaveChangesAsync();
        }
    }
}
