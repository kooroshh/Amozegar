using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class EditOptionViewModel
    {

        [Display(Name = "گزینه")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از 500 کاراکتر باشد")]
        public string Option { get; set; }
    }
}
