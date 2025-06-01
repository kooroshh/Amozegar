using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class EditClassViewModel
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
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string ?ClassPassword { get; set; }

        [Display(Name = "پسورد جدید کلاس")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "تکرار پسورد جدید")]
        [Compare("NewPassword", ErrorMessage = "فیلد {0} با پسورد همخوانی ندارد")]
        [DataType(DataType.Password)]
        public string ?ConfirmNewPassword { get; set; } 

        public string? ImagePath { get; set; }


        [Display(Name = "تصویر")]
        [AllowedExtensions(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSize(4)]
        public IFormFile? ClassImage { get; set; }
    }
}
