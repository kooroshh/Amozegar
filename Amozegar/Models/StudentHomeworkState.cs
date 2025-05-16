using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class StudentHomeworkState
    {
        [Key]
        public int StudentHomeworkStateId { get; set; }

        [MaxLength(255)]
        public string State { get; set; }

        [MaxLength(255)]
        public string PersianState { get; set; }


        public ICollection<StudentHomework> StudentsHomeworks { get; set; }
    }
}
