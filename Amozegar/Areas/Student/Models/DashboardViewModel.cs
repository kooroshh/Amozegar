namespace Amozegar.Areas.Student.Models
{
    public class DashboardViewModel
    {
        public int StudentsCount { get; set; }
        public int NotReadNotificationsCount { get; set; }
        public int NotSentHomeworksCount { get; set; }
        public int OngoingExamsCount { get; set; }
    }
}
