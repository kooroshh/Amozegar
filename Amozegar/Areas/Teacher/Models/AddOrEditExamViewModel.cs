using Amozegar.Areas.Teacher.Models.Interface;
using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class AddOrEditExamViewModel : IExamDateInput
    {
        [Display(Name = "عنوان امتحان")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string ExamTitle { get; set; }

        [Display(Name = "توضیحات امتحان")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(800, ErrorMessage = "{0} نمیتواند بیشتر از 800 کاراکتر باشد")]
        public string ExamDescription { get; set; }

        [Display(Name = "تاریخ شروع امتحان", Prompt = "مثلاً 1403/03/15")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [AllowedInput("####/##/##", "{0} صحیح نمیباشد. نمونه : 1404/04/04")]
        public string StartDate { get; set; }

        [Display(Name = "زمان شروع امتحان", Prompt = "مثلاً 00:00")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [AllowedInput("##:##", "{0} صحیح نمیباشد. نمونه : 00:00")]
        public string StartTime { get; set; }

        [Display(Name = "تاریخ پایان امتحان", Prompt = "مثلاً 1403/03/15")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [AllowedInput("####/##/##", "{0} صحیح نمیباشد. نمونه : 1404/04/04")]
        public string EndDate { get; set; }

        [Display(Name = "زمان پایان امتحان", Prompt = "مثلاً 00:00")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [AllowedInput("##:##", "{0} صحیح نمیباشد. نمونه : 00:00")]
        public string EndTime { get; set; }

        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string? State { get; set; }

    }
}
