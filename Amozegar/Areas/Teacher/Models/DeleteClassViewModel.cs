using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class DeleteClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
    }
}
