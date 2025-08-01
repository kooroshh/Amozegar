namespace Amozegar.Areas.Admin.Models
{
    public class StudentResultViewModel
    {
        public string StudentPicturePath { get; set; }
        public string StudentName { get; set; }
        public string ExamStatus { get; set; }
        public int CorrectAwnser { get; set; }
        public int Score { get; set; }
        public string JoindAt { get; set; }
        public int IncorrectAwnser { get; set; }
        public int AwnserCount { get; set; }
    }
}
