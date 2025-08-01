namespace Amozegar.Areas.Admin.Models
{
    public class TicketViewModel
    {
        public int TicketId { get; set; }
        public string TicketSubject { get; set; }
        public string CreatedAt { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public string? Body { get; set; }
    }
}
