using Amozegar.Areas.Student.Models;
using Amozegar.Models;

namespace Amozegar.Areas.Teacher.Models
{
    public class ExamRsultsViewModel
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public string ExamDescription { get; set; }
        public string CreatedAt { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string State { get; set; }
        public int QuestionCount { get; set; }
        public int Joiner { get; set; }
        public int ScoreAvarage { get; set; }
        public int Accepted { get; set; }
        public List<StudentResult> StudentsResults { get; set; }
    }
}
