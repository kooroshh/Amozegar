using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class HomeworkState
    {
        [Key]
        public int HomeworkStateId { get; set; }

        [Required]
        [MaxLength(255)]
        public string State { get; set; }

        [Required]
        [MaxLength(255)]
        public string PersianState { get; set; }


        public ICollection<Homework> Homeworks { get; set; }
    }
}
