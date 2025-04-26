using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Panel.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "پسورد فعلی")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "پسورد جدید")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "تکرار پسورد جدید")]
        [Compare("NewPassword", ErrorMessage = "فید {0} با پسورد همخوانی ندارد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        public string NewConfirmPassword { get; set; }
    }
}
