using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class ChangeHomeworkSentViewModel
    {
        [Required]
        public int StudentToHomeworkId { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
