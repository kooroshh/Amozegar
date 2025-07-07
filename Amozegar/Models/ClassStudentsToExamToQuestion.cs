using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class ClassStudentsToExamToQuestion
    {
        [Key]
        public int ClassStudentsToExamQuestionId { get; set; }

        [Required]
        public int ClassStudentToExamId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Required]
        public bool IsTrueAwnser { get; set; }

        public int SelectedOptionId { get; set; }



        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        [ForeignKey("ClassStudentToExamId")]
        public ClassStudentsToExam ClassStudentsToExam { get; set; }

        [ForeignKey("SelectedOptionId")]
        public QuestionOption SelectedOption { get; set; }
    }
}
