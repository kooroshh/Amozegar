using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class QuestionOption
    {
        [Key]
        public int QuestionOptionId { get; set; }
        
        [Required]
        public int QuestionId { get; set; }

        [MaxLength(500)]
        public string Option { get; set; }



        [ForeignKey("QuestionId")]
        public Question Question { get; set; }

        public ICollection<ClassStudentsToExamToQuestion> ClassStudentsToExamToQuestions { get; set; }

    }
}
