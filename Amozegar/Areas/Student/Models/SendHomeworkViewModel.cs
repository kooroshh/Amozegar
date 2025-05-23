using Amozegar.Areas.Shared.Models;
using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Student.Models
{
    public class SendHomeworkViewModel
    {
        [Required]
        public int HomeworkId { get; set; }

        public string? HomeworkTitle { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Description { get; set; }

        [Display(Name = "تصاویر")]
        [Required(ErrorMessage = "حدقل باید یک تصویر وارد شده باشد")]
        [AllowedExtensionsList(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSizeList(4)]
        public List<IFormFile> Pictures { get; set; }
    }
}
