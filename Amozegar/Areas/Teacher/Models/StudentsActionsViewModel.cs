using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class StudentsActionsViewModel
    {
        [Required]
        public int StudentInClassId { get; set; }
        public string? StudentName { get; set; }
    }
}
