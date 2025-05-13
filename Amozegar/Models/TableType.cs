using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class TableType
    {
        [Key]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Type { get; set; }

        public ICollection<Picture> Picture { get; set; }
        public ICollection<UserView> UserViews { get; set; }
    }
}
