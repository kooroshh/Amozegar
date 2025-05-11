using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        [Required]
        [MaxLength(255)]
        public string PicturePath { get; set; }

        [Required]
        public int PictureTypeId { get; set; }

        [Required]
        public int PictureTypeRecordId { get; set; }


        [ForeignKey("PictureTypeId")]
        public PictureType PictureType { get; set; }
    }
}
