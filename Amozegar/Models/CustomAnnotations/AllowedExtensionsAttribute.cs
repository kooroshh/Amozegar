using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models.CustomAnnotations
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private string[] _extensions;
        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > 0 )
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult($"فایل باید یکی از فرمت‌های {string.Join(", ", _extensions)} باشد.");
                }
            }

            return ValidationResult.Success;
        }
    }


}
