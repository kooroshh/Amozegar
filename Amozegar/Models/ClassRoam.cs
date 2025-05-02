using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Amozegar.Models.CustomAnnotations;

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
        [MaxLength(255)]
        [IsClassIdentity]
        public string ClassIdentity { get; set; }

        [Required]
        [MaxLength(255)]
        public string ClassPassword { get; set; }
        [MaxLength(255)]
        public string? ClassImage { get; set; }

        public DateTime? Date { get; set; }

        public int? CLassStateId { get; set; }

        public ICollection<ClassStudents> StudentToClasses { get; set; }

        [ForeignKey("TeacherId")]
        public User Teacher { get; set; }

        [ForeignKey("CLassStateId")]
        public ClassStates ClassState { get; set; }
    }
}
