namespace Amozegar.Areas.Teacher.Models.Interface
{
    public interface IExamDateInput
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
