using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class ClassRoam
    {
        [Key]
        public int ClassId { get; set; }
        [Required]
        public string TeacherId { get; set; }
        [Required]
        [MaxLength(255)]
        public string ClassName { get; set; }
        [Required]
        [MaxLength(64)]
        public string ClassPassword { get; set; }

        public ICollection<StudentToClass> StudentToClasses { get; set; }
        [ForeignKey("TeacherId")]
        public User Teacher { get; set; }
    }
}
