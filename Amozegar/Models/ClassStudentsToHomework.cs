using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class ClassStudentsToHomework
    {
        [Key]
        public int ClassStudentsToHomeworkId { get; set; }

        [Required]
        public int HomeworkId { get; set; }

        [Required]
        public int ClassStudentId { get; set; }

        [Required]
        public int ClassStudentHomeworkStateId { get; set; }

        [MaxLength(255)]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? SendAt { get; set; }


        [ForeignKey("HomeworkId")]
        public Homework Homework { get; set; }

        [ForeignKey("ClassStudentHomeworkStateId")]
        public ClassStudentsToHomeworkState ClassStudentsToHomeworkState { get; set; }

        [ForeignKey("ClassStudentId")]
        public ClassStudents ClassStudent { get; set; }
    }
}
