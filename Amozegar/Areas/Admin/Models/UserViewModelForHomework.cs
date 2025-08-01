namespace Amozegar.Areas.Admin.Models
{
    public class UserViewModelForHomework
    {
        public int? StudentToHomeworkId { get; set; }
        public string StudentName { get; set; }
        public string PicturePath { get; set; }
        public string StudentStatus { get; set; }
        public bool IsSent { get; set; }
    }
}
