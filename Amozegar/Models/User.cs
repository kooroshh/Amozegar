using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Amozegar.Models
{
    public class User : IdentityUser
    {
        public DateTime? Date { get; set; }
        public string? PicturePath { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }

        public ICollection<ClassStudents> StudentToClasses { get; set; }
        public ICollection<UserView> UserViews { get; set; }
    }
}
