using System.ComponentModel.DataAnnotations;
using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class AddOrEditNotificaionsViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "بدنه")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Body { get; set; }

        [Display(Name = "تصاویر")]
        [AllowedExtensionsList(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSizeList(4)]
        public List<IFormFile>? Pictures { get; set; }
        public List<PictureForEditViewModel>? PicturesList { get; set; }
    }
}
