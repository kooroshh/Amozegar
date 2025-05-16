using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int HomeworkStateId { get; set; }

        [Required]
        [MaxLength(255)]
        public string HomeworkTitle { get; set; }

        [Required]
        public string HomeworkDescription { get; set; }

        public DateTime? CreatedAt { get; set; }


        [ForeignKey("ClassId")]
        public ClassRoam ClassRoam { get; set; }

        [ForeignKey("HomeworkStateId")]
        public HomeworkState HomeworkState { get; set; }

        public ICollection<StudentHomework> StudentHomeworks { get; set; }
    }
}