using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        [MaxLength(500)]
        public string QuestionAsk { get; set; }

        [Required]
        [MaxLength(500)]
        public string Answer { get; set; }

        public DateTime? CreatedAt { get; set; }


        [ForeignKey("ExamId")]
        public Exam Exam { get; set; }

        public ICollection<QuestionOption> QuestionOptions { get; set; }
        public ICollection<ClassStudentsToExamToQuestion> ClassStudentsToExamToQuestions { get; set; }

    }
}
