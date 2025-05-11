using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        public int ClassId { get; set; }

        [MaxLength(255)]
        [Required]
        public string NotificationTitle { get; set; }

        [Required]
        public string NotificationBody { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("ClassId")]
        public ClassRoam ClassRoam { get; set; }
    }
}
