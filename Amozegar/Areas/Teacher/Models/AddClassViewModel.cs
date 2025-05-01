using Amozegar.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Amozegar.Models.CustomAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class AddClassViewModel
    {
        [Display(Name = "ایدی کلاس")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "{0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [IsClassIdentity]
        public string ClassIdentity { get; set; }

        [Display(Name = "نام کلاس")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "{0} نمیتواند کمتر از 3 کاراکتر باشد")]
        public string ClassName { get; set; }

        [Display(Name = "پسورد کلاس")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string ClassPassword { get; set; }

        [Display(Name = "تکرار پسورد")]
        [Compare("ClassPassword", ErrorMessage = "فیلد {0} با پسورد همخوانی ندارد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "تصویر")]
        [AllowedExtensions(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSize(4)]
        public IFormFile? ClassImage { get; set; }

    }
}
