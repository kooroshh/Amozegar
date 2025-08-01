using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Admin.Models
{
    public class EditRoleViewModel
    {
        [Display(Name = "نقش ها")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(1, ErrorMessage = "{0} نمیتواند کمتر از 1 گزینه باشد")]
        [MaxLength(3, ErrorMessage = "{0} نمیتواند بیشتر از 3 گزینه باشد")]
        [NoDuplicates]
        public List<string> NewRoles { get; set; } = new();
    }
}
