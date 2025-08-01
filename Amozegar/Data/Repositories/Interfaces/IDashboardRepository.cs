namespace Amozegar.Data.Repositories.Interfaces
{
    public interface IDashboardRepository : IGenericRepository<object>
    {
        Task<Areas.Admin.Models.DashboardViewModel> GetAdminDashboardDatasAsync();
        Task<Areas.Teacher.Models.DashboardViewModel> GetTeacherDashboardDatasByClassIdentityAsync(string classIdentity);
        Task<Areas.Student.Models.DashboardViewModel> GetStudentDashboardDatasByClassIdentityAsync(string classIdentity, string userId);
    }
}
