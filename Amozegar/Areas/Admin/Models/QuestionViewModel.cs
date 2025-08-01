namespace Amozegar.Areas.Admin.Models
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public string ClassIdentity { get; set; }
        public string Question { get; set; }
        public string CreatedAt { get; set; }
        public IEnumerable<OptionViewModel> Options { get; set; }
        public int OptionsCount { get; set; }
    }
}
