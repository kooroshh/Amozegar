using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class ClassStudentsToExam
    {
        [Key]
        public int ClassStudentsToExamId { get; set; }

        [Required]
        public int ClassStudentId { get; set; }

        [Required]
        public int ExamId { get; set; }

        public DateTime? JoinAt { get; set; }

        public bool IsFinish { get; set; }

        public int LastCompletedQuestion { get; set; }



        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        [ForeignKey("ClassStudentId")]
        public ClassStudents ClassStudent { get; set; }

        public ICollection<ClassStudentsToExamToQuestion> ClassStudentsToExamToQuestions { get; set; }
    }
}
