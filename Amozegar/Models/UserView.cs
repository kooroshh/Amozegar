using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class UserView
    {
        [Key]
        public int UserViewId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TableTypeId { get; set; }

        [Required]
        public int TableTypeRecordId { get; set; }


        [ForeignKey("TableTypeId")]
        public TableType TableType { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
