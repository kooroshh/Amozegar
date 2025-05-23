namespace Amozegar.Areas.Shared.Models
{
    public class HomeworksViewModel
    {
        public int HomeworkId { get; set; }
        public string HomewordTitle { get; set; }
        public string CreatedAt { get; set; }
        public string State { get; set; }
        public string? PersianStudentState { get; set; }
        public string? StudentState { get; set; }
    }
}
