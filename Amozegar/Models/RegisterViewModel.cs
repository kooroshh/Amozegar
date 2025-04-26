using System.ComponentModel.DataAnnotations;
using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Amozegar.Models
{
    public class RegisterViewModel
    {
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل صحیح نمیباشد")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string Email { get; set; }
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string FullName { get; set; }

        [Display(Name = "پسورد")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تکرار پسورد")]
        [Compare("Password", ErrorMessage = "فید {0} با پسورد همخوانی ندارد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "نوع دسترسی")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        public string SelectedRole { get; set; }

        public List<SelectListItem>? Roles { get; set; }

        [Display(Name = "تصویر")]
        [AllowedExtensions(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSize(4)]
        public IFormFile? UserPicture { get; set; }
    }
}
