using Amozegar.Data.Repositories;
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
        Task SaveChangesAsync();
    }
}
