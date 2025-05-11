using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class DeleteNotificationViewModel
    {

        [Required]
        public int NotificationId { get; set; }

        public string? NotificationTitle { get; set; }

    }
}
