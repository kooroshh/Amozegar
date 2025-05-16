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
        public int ClassId { get; set; }

        [Required]
        public int TableTypeId { get; set; }

        [Required]
        public int TableTypeRecordId { get; set; }


        [ForeignKey("TableTypeId")]
        public TableType TableType { get; set; }

        [ForeignKey("ClassId")]
        public ClassRoam ClassRoam { get; set; }
    }
}
