using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amozegar.Models
{
    public class StudentToClass
    {
        public string StudentId { get; set; }
        public int ClassId { get; set; }


        [ForeignKey("StudentId")]
        public User User { get; set; }
        [ForeignKey("ClassId")]
        public ClassRoam Class { get; set; }
    }
}
