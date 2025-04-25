using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class LoginViewModel
    {
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "آدرس ایمیل صحیح نمیباشد")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string Email { get; set; }

        [Display(Name = "پسورد")]
        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [MinLength(3, ErrorMessage = "فیلد {0} نمیتواند کمتر از 3 کاراکتر باشد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
