using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class QuestionEditViewModel
    {
        public int QuestionId { get; set; }

        [Display(Name = "متن سوال", Prompt = "متن سوال خود را وارد کنید...")]
        public string QuestionAsk { get; set; }

        public List<QuestionOptionsForEditViewModel> Options { get; set; } = new();
    }
}
