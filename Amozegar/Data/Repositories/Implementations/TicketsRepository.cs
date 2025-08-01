using Amozegar.Areas.Admin.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class TicketsRepository : GenericRepository<Report>, ITicketsRepository
    {
        public TicketsRepository(AmozegarContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TicketViewModel>> GetTicketsByPageNumebrAsync(int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var tickets = await this._context.Reports
                .Select(r => new TicketViewModel()
                {
                    CreatedAt = r.Date.ToShamsi(),
                    TicketId = r.ReportId,
                    TicketSubject = r.Subject,
                    UserEmail = r.Email,
                    UserFullName = r.FullName,
                    UserPhoneNumber = r.PhoneNumber,
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return tickets;
        }

        public async Task<int> TicketCountAsync()
        {
            var count = await this._context.Reports
                .CountAsync();

            return count;
        }
    }
}
