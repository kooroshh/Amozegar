using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class DashboardRepository : GenericRepository<object>, IDashboardRepository
    {
        public DashboardRepository(AmozegarContext context) : base(context)
        {
        }

        // Utilities


        private async Task<int> getUsersCountByRole(string role)
        {
            var count = await _context.Users
                .Where(u => _context.UserRoles
                    .Any(ur =>
                        ur.UserId == u.Id &&
                        _context.Roles
                            .Any(r => r.Id == ur.RoleId && r.Name == role)
                    )
                )
                .CountAsync();
            return count;
        }

        private async Task<int> getClassIdByClassIdentityAsync(string classIdentity)
        {
            var clsId = await this._context.Classes
                .Where(c => c.ClassIdentity == classIdentity)
                .Select(c => c.ClassId)
                .SingleAsync();

            return clsId;
        }


        // Main Methods

        public async Task<Areas.Admin.Models.DashboardViewModel> GetAdminDashboardDatasAsync()
        {

            var viewMdodel = new Areas.Admin.Models.DashboardViewModel()
            {
                AdminsCount = await this.getUsersCountByRole("Admin"),
                StudentsCount = await this.getUsersCountByRole("Student"),
                TeachersCount = await this.getUsersCountByRole("Teacher"),
                UsersCount = await this._context.Users.CountAsync(),
                BannedUsersCount = await this._context.Users.CountAsync(u => u.LockoutEnd != null && u.LockoutEnd > DateTime.Now),
                NotBannedUsersCount = await this._context.Users.CountAsync(u => u.LockoutEnd == null || u.LockoutEnd < DateTime.Now),
                TicketsCount = await this._context.Reports.CountAsync(),
                ClassesCount = await this._context.Classes.CountAsync(c => c.ClassState.State != "Deleted"),
                BannedClassesCount = await this._context.Classes.CountAsync(c => c.ClassState.State == "Banned"),
                NotBannedClassesCount = await this._context.Classes.CountAsync(c => c.ClassState.State == "Active"),
                NotificationsCount = await this._context.Notifications.CountAsync(),
                HomewroksCount = await this._context.Homeworks.CountAsync(h => h.HomeworkState.State != "Deleted"),
                ClosedHomeworksCount = await this._context.Homeworks.CountAsync(h => h.HomeworkState.State == "Closed"),
                OpenHomeworksCount = await this._context.Homeworks.CountAsync(h => h.HomeworkState.State == "Open"),
                ExamsCount = await this._context.Exams.CountAsync(e => e.ExamState.State != "Deleted"),
                OngoingExamsCount = await this._context.Exams.CountAsync(e => e.ExamState.State == "Ongoing"),
                DraftExamsCount = await this._context.Exams.CountAsync(e => e.ExamState.State == "Draft"),
                CompletedExamsCount = await this._context.Exams.CountAsync(e => e.ExamState.State == "Completed"),
                ScheduledExamsCount = await this._context.Exams.CountAsync(e => e.ExamState.State == "Scheduled"),
                ClosedExamsCount = await this._context.Exams.CountAsync(e => e.ExamState.State == "Closed"),
                QuestionsCount = await this._context.Questions.CountAsync(q => q.Exam.ExamState.State != "Deleted"),
                OptionsCount = await this._context.QuestionOptions.CountAsync(qo => qo.Question.Exam.ExamState.State != "Deleted"),
            };

            return viewMdodel;
        }

        public async Task<Areas.Teacher.Models.DashboardViewModel> GetTeacherDashboardDatasByClassIdentityAsync(string classIdentity)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);
            var viewMdodel = new Areas.Teacher.Models.DashboardViewModel()
            {
                StudentsCount = await this._context.ClassesStudents.CountAsync(cs => cs.State.State == "Accepted" && cs.ClassId == clsId),
                BanndedStudentsCount = await this._context.ClassesStudents.CountAsync(cs => cs.State.State == "Banned" && cs.ClassId == clsId),
                LoginsToClassCount = await this._context.ClassesStudents.CountAsync(cs => cs.ClassId == clsId && cs.State.State == "Pending"),
                NotificationsCount = await this._context.Notifications.CountAsync(n => n.ClassId == clsId),
                ExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State != "Deleted"),
                OngoingExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State == "Ongoing"),
                ClosedExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State == "Closed"),
                CompletedExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State == "Completed"),
                DraftExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State == "Draft"),
                ScheduledExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State == "Scheduled"),
                HomeworksCount = await this._context.Homeworks.CountAsync(h => h.ClassId == clsId && h.HomeworkState.State != "Deleted"),
                ClosedHomeworksCount = await this._context.Homeworks.CountAsync(h => h.ClassId == clsId && h.HomeworkState.State == "Closed"),
                OpenHomeworksCount = await this._context.Homeworks.CountAsync(h => h.ClassId == clsId && h.HomeworkState.State == "Open"),
                HomeworkSentsCount = await this._context.ClassStudentsToHomeworks
                    .CountAsync(csth =>
                        csth.Homework.ClassId == clsId &&
                        (csth.ClassStudentsToHomeworkState.State == "Pending" || csth.ClassStudentsToHomeworkState.State == "Resubmitted") &&
                        csth.Homework.HomeworkState.State != "Deleted" &&
                        csth.ClassStudent.State.State == "Accepted"
                    ),
            };

            return viewMdodel;
        }

        public async Task<Areas.Student.Models.DashboardViewModel> GetStudentDashboardDatasByClassIdentityAsync(string classIdentity, string userId)
        {
            var clsId = await this.getClassIdByClassIdentityAsync(classIdentity);
            var readRecords = await this._context.UsersViews
                .Where(uv => uv.TableType.Type == "Notifications" && uv.UserId == userId && uv.ClassId == clsId)
                .Select(uv => uv.TableTypeRecordId)
                .ToListAsync();

            var classStudentId = await this._context.ClassesStudents
                .Where(cs => cs.ClassId == clsId && cs.StudentId == userId)
                .Select(cs => cs.Id)
                .SingleAsync();

            var viewModel = new Areas.Student.Models.DashboardViewModel()
            {
                StudentsCount = await this._context.ClassesStudents.CountAsync(cs => cs.State.State == "Accepted" && cs.ClassId == clsId),
                OngoingExamsCount = await this._context.Exams.CountAsync(e => e.ClassId == clsId && e.ExamState.State == "Ongoing"),
                NotReadNotificationsCount = await this._context.Notifications
                .CountAsync(n => !readRecords.Contains(n.NotificationId) && n.ClassId == clsId),
                NotSentHomeworksCount = await 
                    (
                        from h in _context.Homeworks
                        where h.HomeworkState.State == "Open"
                        join csth in _context.ClassStudentsToHomeworks
                            .Include(x => x.ClassStudentsToHomeworkState)
                            on new { h.HomeworkId, StudentId = classStudentId }
                            equals new { csth.HomeworkId, StudentId = csth.ClassStudentId }
                            into gj
                        from csth in gj.DefaultIfEmpty()
                        where h.ClassId == clsId &&
                              h.HomeworkState.State != "Deleted" &&
                              (csth == null || csth.ClassStudentsToHomeworkState == null || csth.ClassStudentsToHomeworkState.State == "Rejected")
                        select h
                    ).CountAsync()
            };

            return viewModel;
        }
    }
}
