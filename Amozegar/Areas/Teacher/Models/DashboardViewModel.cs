namespace Amozegar.Areas.Teacher.Models
{
    public class DashboardViewModel
    {
        public int LoginsToClassCount { get; set; }
        public int StudentsCount { get; set; }
        public int BanndedStudentsCount { get; set; }
        public int NotificationsCount { get; set; }
        public int HomeworksCount { get; set; }
        public int ClosedHomeworksCount { get; set; }
        public int OpenHomeworksCount { get; set; }
        public int HomeworkSentsCount { get; set; }
        public int ExamsCount { get; set; }
        public int OngoingExamsCount { get; set; }
        public int ClosedExamsCount { get; set; }
        public int CompletedExamsCount { get; set; }
        public int DraftExamsCount { get; set; }
        public int ScheduledExamsCount { get; set; }
    }
}
