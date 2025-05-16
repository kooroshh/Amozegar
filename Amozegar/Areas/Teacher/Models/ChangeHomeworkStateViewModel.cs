using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class ChangeHomeworkStateViewModel
    {
        [Required]
        public int HomeworkId { get; set; }

        [Required]
        public string? HomeworkTitle { get; set; }
    }
}
