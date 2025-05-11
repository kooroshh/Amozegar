using System.ComponentModel.DataAnnotations;

namespace Amozegar.Models.CustomAnnotations
{
    public class AllowedExtensionsListAttribute : ValidationAttribute
    {
        private string[] _extensions;
        public AllowedExtensionsListAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var files = value as IEnumerable<IFormFile>;
            if (files is not IEnumerable<IFormFile>)
            {
                return ValidationResult.Success;
            }
            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_extensions.Contains(extension))
                    {
                        return new ValidationResult($"فایل باید یکی از فرمت‌های {string.Join(", ", _extensions)} باشد.");
                    }
                }
            }


            return ValidationResult.Success;
        }
    }
}
