using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        [MaxLength(255)]
        public string ExamTitle { get; set; }

        [Required]
        [MaxLength(800)]
        public string ExamDescription { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public int ExamStateId { get; set; }


        [ForeignKey("ClassId")]
        public ClassRoam ClassRoam { get; set; }

        [ForeignKey("ExamStateId")]
        public ExamState ExamState { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<ClassStudentsToExam> ClassStudentsToExam { get; set; }
    }
}
