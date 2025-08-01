namespace Amozegar.Areas.Admin.Models
{
    public class ExamViewModel
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDescription { get; set; }
        public string PersianState { get; set; }
        public int QuestionCount { get; set; }
        public string ClassIdentity { get; set; }
        public string CreatedAt { get; set; }
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public int JoinerCount { get; set; }
        public int ScoreAvarage { get; set; }
        public int AcceptedCount { get; set; }
        public IEnumerable<QuestionsViewModel> Questions { get; set; }
        public IEnumerable<StudentResultViewModel> Students { get; set; }
    }
}
