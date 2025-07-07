using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class AddOptionViewModel : IValidatableObject
    {
        public int QuestionId { get; set; }

        [Display(Name = "گزینه ها")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(1, ErrorMessage = "{0} نمیتواند کمتر از 1 گزینه باشد")]
        [NoDuplicates]
        public List<string> Options { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Options?.Count() > 0 && Options.Any(o => string.IsNullOrEmpty(o) || o.Length > 500))
            {
                yield return new ValidationResult(
                    "گزینه ها نمیتوانند خالی یا بزرگ تر از 500 کاراکتر باشند",
                    new[] { nameof(Options) }
                );
            }

        }
    }
}
