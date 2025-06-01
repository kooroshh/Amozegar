using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Shared.Models
{
    public class AddPictureViewModel
    {
        [Display(Name = "تصاویر")]
        [Required(ErrorMessage = "هیچ عکسی انتخاب نشده است")]
        [AllowedExtensionsList(".jpg", ".jpeg", ".png", ".gif")]
        [MaxFileSizeList(4)]
        public List<IFormFile> Pictures { get; set; }
    }
}
