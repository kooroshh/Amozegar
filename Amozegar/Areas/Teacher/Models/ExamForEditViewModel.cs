using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Amozegar.Areas.Teacher.Models
{
    public class ExamForEditViewModel
    {
        [Display(Name = "عنوان امتحان")]
        public string? ExamTitle { get; set; }

        [Display(Name = "توضیحات امتحان")]
        public string? ExamDescription { get; set; }

        [Display(Name = "تاریخ شروع")]
        public string? StartDate { get; set; }

        [Display(Name = "زمان شروع")]
        public string? StartTime { get; set; }

        [Display(Name = "تاریخ پایان")]
        public string? EndDate { get; set; }

        [Display(Name = "زمان پایان")]
        public string? EndTime { get; set; }
        public IEnumerable<SelectListItem>? States { get; set; }
        public List<QuestionForEditViewModel> Questions { get; set; } = new();
    }
}
