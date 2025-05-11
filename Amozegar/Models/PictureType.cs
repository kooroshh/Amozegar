using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class PictureType
    {
        [Key]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Type { get; set; }

        public ICollection<Picture> Picture { get; set; }
    }
}
