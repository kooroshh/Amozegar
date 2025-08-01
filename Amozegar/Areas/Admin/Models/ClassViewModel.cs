namespace Amozegar.Areas.Admin.Models
{
    public class ClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassIdentity { get; set; }
        public string TeacherName { get; set; }
        public string ClassImage { get; set; }
        public string CreatedAt { get; set; }
        public string PersianState { get; set; }
        public string State { get; set; }
        public int NotificationsCount { get; set; }
        public int HomeworksCount { get; set; }
        public int ExamsCount { get; set; }
        public int StudentsCount { get; set; }
        public List<UserViewModelForClass> Students { get; set; } = new();
    }
}
