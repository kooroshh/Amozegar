using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class ExamState
    {
        [Key]
        public int ExamStateId { get; set; }

        [Required]
        [MaxLength(255)]
        public string State { get; set; }

        [Required]
        [MaxLength(255)]
        public string PersianState { get; set; }


        public ICollection<Exam> Exams { get; set; }
    }
}
