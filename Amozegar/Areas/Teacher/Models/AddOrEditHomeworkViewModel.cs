using Amozegar.Areas.Shared.Models;
using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class AddOrEditHomeworkViewModel
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(255, ErrorMessage = "{0} نمیتواند بیشتر از 255 کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public string Description { get; set; }

        [Display(Name = "تصاویر")]
        [AllowedExtensionsList(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSizeList(4)]
        public List<IFormFile>? Pictures { get; set; }
        public List<PictureForEditViewModel>? PicturesList { get; set; }
    }
}
