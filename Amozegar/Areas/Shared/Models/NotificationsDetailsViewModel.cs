namespace Amozegar.Areas.Shared.Models
{
    public class NotificationsDetailsViewModel
    {
        public int? NotificationId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationBody { get; set; }
        public string CreatedAt { get; set; }
        public List<string> PicturePaths { get; set; }
    }
}
