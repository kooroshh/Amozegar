using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models.CustomAnnotations
{
    public class NoDuplicatesAttribute : ValidationAttribute
    {
        public NoDuplicatesAttribute()
        {
            ErrorMessage = "مقادیر تکراری مجاز نیستند.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<string> enumerable)
            {
                var list = enumerable.Where(x => x != null).ToList();

                // بررسی آیتم‌های تکراری
                bool hasDuplicates = list.Count != list.Distinct().Count();

                if (hasDuplicates)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
