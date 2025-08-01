namespace Amozegar.Areas.Admin.Models
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationBody { get; set; }
        public string CreatedAt { get; set; }
        public string ClassIdentity { get; set; }
        public List<string> PicturePaths { get; set; }
    }
}
