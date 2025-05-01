using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Amozegar.Models
{
    public class UserRole : IdentityRole
    {
        public UserRole() : base() { }
        public UserRole(string roleName) : base(roleName) { }
        public UserRole(string roleName, string persianName) : base(roleName)
        {
            this.PersianName = persianName;
        }


        [MaxLength(255)]
        public string? PersianName { get; set; }
    }
}
