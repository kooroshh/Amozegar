namespace Amozegar.Areas.Shared.Models
{
    public class ExamsViewModel
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public string CreatedAt { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public int QuestionsCount { get; set; }
        public string PersianState { get; set; }
        public string State { get; set; }
        public string? Description { get; set; }

    }
}
