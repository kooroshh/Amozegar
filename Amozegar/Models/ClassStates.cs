using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class ClassStates
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(255)]
        public string State { get; set; }

        [Required]
        [MaxLength(255)]
        public string PersianState { get; set; }

        public ICollection<ClassRoam> ClassRoam { get; set; }
    }
}
