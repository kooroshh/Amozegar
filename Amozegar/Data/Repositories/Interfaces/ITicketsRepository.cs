using Amozegar.Areas.Admin.Models;
using Amozegar.Models;

namespace Amozegar.Data.Repositories.Interfaces
{
    public interface ITicketsRepository : IGenericRepository<Report>
    {
        Task<IEnumerable<TicketViewModel>> GetTicketsByPageNumebrAsync(int pageNumber);
        Task<int> TicketCountAsync();
    }
}
