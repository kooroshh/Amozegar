using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models.CustomAnnotations
{
    public class MaxFileSizeListAttribute : ValidationAttribute
    {
        private int _maxFileSizeInBytes;
        public MaxFileSizeListAttribute(int maxFileSizeInBytes)
        {
            _maxFileSizeInBytes = maxFileSizeInBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files is not IEnumerable<IFormFile>)
            {
                return ValidationResult.Success;
            }

            foreach(var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    if (file.Length > (_maxFileSizeInBytes * 1024 * 1024))
                    {
                        return new ValidationResult($"حجم فایل نباید بیشتر از {_maxFileSizeInBytes} مگابایت باشد.");
                    }
                }
            }


            return ValidationResult.Success;
        }
    }
}
