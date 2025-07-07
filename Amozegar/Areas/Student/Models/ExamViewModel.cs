using Amozegar.Models;

namespace Amozegar.Areas.Student.Models
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDescription { get; set; }
        public string CreatedAt { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public ExamState State { get; set; }
        public int QuestionCount { get; set; }
        public ExamResultViewModel? ExamResult { get; set; }
    }
}
