using Amozegar.Models.CustomAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class AddQuestionViewModel : IValidatableObject
    {
        public int ExamId { get; set; }

        [Display(Name = "سوال")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از 500 کاراکتر باشد")]
        public string QuestionAsk { get; set; }

        [Display(Name = "گزینه ها")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        [MinLength(2, ErrorMessage = "{0} نمیتواند کمتر از 2 گزینه باشد")]
        [NoDuplicates]
        public List<string> Options { get; set; } = new();

        [Display(Name = "جواب")]
        [Required(ErrorMessage = "لطفا حدقل یک گزینه را به عنوان {0} انتخاب کنید")]
        public string CorrectAnswer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Options?.Count > 0 && !Options.Contains(CorrectAnswer?.Trim(), StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    "جواب صحیح باید یکی از گزینه‌ها باشد.",
                    new[] { nameof(CorrectAnswer) }
                );
            }
        }

    }
}
