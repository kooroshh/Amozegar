using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models.CustomAnnotations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private int _maxFileSizeInBytes;
        public MaxFileSizeAttribute(int maxFileSizeInBytes)
        {
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > 0)
            {
                if (file.Length > (_maxFileSizeInBytes * 1024 * 1024))
                {
                    return new ValidationResult($"حجم فایل نباید بیشتر از {_maxFileSizeInBytes / (1024 * 1024)} مگابایت باشد.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
