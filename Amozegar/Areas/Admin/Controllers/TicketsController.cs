using Amozegar.Areas.Admin.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin/Tickets")]
    public class TicketsController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public TicketsController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        // Utilities

        private IActionResult RedirectToTickets() => RedirectToAction("Index", "Tickets", new { pageNumber = 1 });

        private IActionResult RedirectToTickets(string error)
        {
            TempData["Error"] = error;
            return RedirectToAction("Index", "Tickets", new { pageNumber = 1 });
        }


        // Main Methods

        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int pageNumber)
        {
            ViewBag.Route = "Tickets";

            var tickets = await this._context.TicketsRepository
                .GetTicketsByPageNumebrAsync(pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, tickets.Count()))
            {
                return this.RedirectToTickets();
            }

            var ticketCount = await this._context.TicketsRepository
                .TicketCountAsync();

            this.checkNextOrPrevForViewBags(ticketCount, pageNumber);

            var user = await this._userManager.FindByEmailAsync(User.Identity.Name);

            await this._context.UsersViewsRepository
                .ReadAllTicketsAsync(user);

            return View(tickets);
        }

        [HttpPost("DeleteTicket/{ticketId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTicket(int ticketId)
        {
            var ticket = await this._context.TicketsRepository
                .GetByIdAsync(ticketId);

            if (ticket == null)
            {
                return this.RedirectToTickets("چنین تیکتی وجود ندارد");
            }

            this._context.TicketsRepository.Delete(ticket);

            await this._context.SaveChangesAsync();

            return RedirectToTickets();

        }

        [Route("ShowTicket/{ticketId}")]
        public async Task<IActionResult> ShowTicket(int ticketId)
        {
            ViewBag.Route = "Tickets";
            var ticket = await this._context.TicketsRepository
                .GetByIdAsync(ticketId);

            if (ticket == null)
            {
                return this.RedirectToTickets("چنین تیکتی وجود ندارد");
            }

            var ticketViewModel = new TicketViewModel()
            {
                Body = ticket.Message,
                CreatedAt = ticket.Date.ToShamsi(),
                TicketId = ticketId,
                TicketSubject = ticket.Subject,
                UserEmail = ticket.Email,
                UserFullName = ticket.FullName,
                UserPhoneNumber = ticket.PhoneNumber,
            };
            return View(ticketViewModel);

        }


    }
}
