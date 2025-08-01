namespace Amozegar.Areas.Teacher.Models
{
    public class HomeworkSentCheckViewModel
    {
        public int? HomeworkId { get; set; }
        public bool? IsActionable { get; set; }
        public int StudentToHomeworkId { get; set; }
        public string StudentImage { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string HomeworkTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SendAt { get; set; }
        public IEnumerable<string> Pictures { get; set; }
    }
}
