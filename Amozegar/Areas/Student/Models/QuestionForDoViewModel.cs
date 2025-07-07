using Amozegar.Models;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Student.Models
{
    public class QuestionForDoViewModel
    {
        public int? QuestionId { get; set; }
        public string? QuestionAsk { get; set; }
        public List<QuestionOptionForShowViewModel>? Options { get; set; }
        public int? OptionsCount { get; set; }
        public int? QuestionsCount { get; set; }
        public int? CurrentQuestionIndex { get; set; }

        [Display(Name = "جواب")]
        [Required(ErrorMessage = "{0} نمیتواند خالی باشد")]
        public int AwnserOptionId { get; set; }
    }
}
