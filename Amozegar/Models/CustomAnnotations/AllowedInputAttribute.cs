using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Amozegar.Models.CustomAnnotations
{
    public class AllowedInputAttribute : ValidationAttribute
    {
        private string _pattern;
        private Regex _compiledRegex;

        public AllowedInputAttribute(string pattern, string errorMessage = "")
        {
            _pattern = pattern;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                this.ErrorMessage = errorMessage;
            }
            else
            {
                ErrorMessage = $"فرمت وارد شده معتبر نیست. الگوی صحیح: {pattern}";
            }
            _compiledRegex = new Regex(this.convertToRegexPattern(pattern));
        }

        // Utilities

        private string convertToRegexPattern(string pattern)
        {
            var sb = new StringBuilder("^");
            foreach (char c in pattern)
            {
                if (c == '#')
                    sb.Append("\\d");
                else
                    sb.Append(Regex.Escape(c.ToString()));
            }
            sb.Append("$");
            return sb.ToString();
        }

        // Main Methods

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var input = value as string;
            if (string.IsNullOrWhiteSpace(input))
                return true;

            if (Regex.IsMatch(input, "-"))
                return false;

            return _compiledRegex.IsMatch(input);
        }
    }
}
