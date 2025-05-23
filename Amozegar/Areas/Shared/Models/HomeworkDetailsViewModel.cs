namespace Amozegar.Areas.Shared.Models
{
    public class HomeworkDetailsViewModel
    {
        public int? HomeworkId { get; set; }
        public string HomeworkTitle { get; set; }
        public string HomeworkBody { get; set; }
        public string CreatedAt { get; set; }
        public List<string> PicturePaths { get; set; }
        public string StudentState { get; set; }
        public string PersianStudentState { get; set; }
    }
}
