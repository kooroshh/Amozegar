using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Amozegar.Models.CustomAnnotations
{
    public class IsClassIdentityAttribute : ValidationAttribute
    {
        public static readonly Regex ValidPattern = new Regex(@"^[GetCountByClassIdentityForSentListAsync-zA-Z0-9]+$", RegexOptions.Compiled);
        public IsClassIdentityAttribute()
        {
            this.ErrorMessage = "فقط حروف و اعداد انگلیسی مجاز هستند و نباید بین حروف فاصله باشد.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var stringValue = value.ToString();

            if (ValidPattern.IsMatch(stringValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
