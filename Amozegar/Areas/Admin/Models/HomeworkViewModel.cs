namespace Amozegar.Areas.Admin.Models
{
    public class HomeworkViewModel
    {
        public int HomeworkId { get; set; }
        public int ClassId { get; set; }
        public string HomeworlTitle { get; set; }
        public string ClassIdentity { get; set; }
        public string HomeworkBody { get; set; }
        public string HomeworkState { get; set; }
        public string State { get; set; }
        public string CreatedAt { get; set; }
        public List<string> PicturesPath { get; set; }
        public IEnumerable<UserViewModelForHomework> Students { get; set; }
    }
}
