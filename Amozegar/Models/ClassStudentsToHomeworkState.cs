using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class ClassStudentsToHomeworkState
    {
        [Key]
        public int ClassStudentsToHomeworkStateId { get; set; }

        [MaxLength(255)]
        public string State { get; set; }

        [MaxLength(255)]
        public string PersianState { get; set; }


        public ICollection<ClassStudentsToHomework> ClassStudentsToHomeworks { get; set; }
    }
}
