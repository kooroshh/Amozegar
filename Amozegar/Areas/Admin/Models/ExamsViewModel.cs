namespace Amozegar.Areas.Admin.Models
{
    public class ExamsViewModel
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string ClassIdentity { get; set; }
        public string CreatedAt { get; set; }
        public string State { get; set; }
        public string PersianState { get; set; }
        public int QuestionCount { get; set; }
    }
}
