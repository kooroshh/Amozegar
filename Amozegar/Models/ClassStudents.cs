using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class ClassStudents
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int ClassStudentStateId { get; set; }


        [ForeignKey("StudentId")]
        public User User { get; set; }

        [ForeignKey("ClassId")]
        public ClassRoam Class { get; set; }

        [ForeignKey("ClassStudentStateId")]
        public ClassStudentState State { get; set; }

        public ICollection<ClassStudentsToHomework> ClassStudentsToHomeworks { get; set; }
    }
}
