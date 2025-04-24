using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "فیلد {0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "فیلد {0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [EmailAddress(ErrorMessage = "فیلد {0} اشتباه است")]
        [Display(Name = "آدرس ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "{0} معتبر نیست")]
        [MaxLength(11, ErrorMessage = "{0} نمیتواند بیشتر از 11 رقم باشد")]
        [MinLength(11, ErrorMessage = "{0} نمیتواند کمتر از 11 رقم باشد({0} باید با 0 شروع شود)")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "فیلد {0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "فیلد {0} نمیتواند خالی باشد")]
        [Display(Name = "پیام")]
        public string Message { get; set; }

        public DateTime? Date { get; set; }
    }
}
