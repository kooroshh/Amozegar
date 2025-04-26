using System.ComponentModel.DataAnnotations;
using Amozegar.Models.CustomAnnotations;

namespace Amozegar.Areas.Panel.Models
{
    public class EditInformationsViewModel
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "تصویر")]
        [AllowedExtensions(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSize(4)]
        public IFormFile? UserPicture { get; set; }
    }
}
