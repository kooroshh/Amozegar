using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class StudentHomework
    {
        [Key]
        public int StudentHomeworkId { get; set; }

        [Required]
        public int HomeworkId { get; set; }

        [Required]
        public int ClassStudentId { get; set; }

        [Required]
        public int StudentHomeworkStateId { get; set; }


        [ForeignKey("HomeworkId")]
        public Homework Homework { get; set; }

        [ForeignKey("StudentHomeworkStateId")]
        public StudentHomeworkState StudentHomeworkState { get; set; }

        [ForeignKey("ClassStudentId")]
        public ClassStudents ClassStudent { get; set; }
    }
}
