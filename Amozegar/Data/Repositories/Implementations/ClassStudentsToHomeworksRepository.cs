using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassStudentsToHomeworksRepository : GenericRepository<ClassStudentsToHomework>, IClassStudentsToHomeworksRepository
    {
        private UserManager<User> _userManager;
        public ClassStudentsToHomeworksRepository(AmozegarContext context, UserManager<User> userManager) : base(context)
        {
            this._userManager = userManager;
        }

        // private classes

        private class StudentToHomeworksForHomeworksSentViewModel
        {
            public string HomeworkTitle { get; set; }
            public int StudentToHomeworkId { get; set; }
            public string StudentId { get; set; }
        }

        // Utilities

        private async Task<int> getClassIdByClassIdentity(string classIdentity)
        {
            var cls = await this._context.Classes
                .SingleAsync(c => c.ClassIdentity == classIdentity);

            return cls.ClassId;
        }

        private async Task<IEnumerable<string>> getPendingsStates()
        {
            var states = await this._context.ClassStudentsToHomeworkStates
                .Where(csths => csths.State == "Pending" || csths.State == "Resubmitted")
                .Select(csth => csth.State)
                .ToListAsync();

            return states;
        }


        // Main Methods
        public async Task<ClassStudentsToHomework?> GetByHomeworkIdByClassStudentIdAsync(int homeworkId, int classStudentId)
        {
            var classStudentToHomework = await this._context.ClassStudentsToHomeworks
                .Include(csth => csth.ClassStudentsToHomeworkState)
                .SingleOrDefaultAsync(csth =>
                    csth.ClassStudentId == classStudentId &&
                    csth.HomeworkId == homeworkId &&
                    csth.Homework.HomeworkState.State != "Deleted"
                );

            return classStudentToHomework;

        }

        public async Task<IEnumerable<HomeworksSentViewModel>> GetByClassIdentityByPageNumberForSentListAsync(string classIdentity, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;
            var clsId = await this.getClassIdByClassIdentity(classIdentity);
            var states = await this.getPendingsStates();


            var classStudentsToHomeworks = await this._context.ClassStudentsToHomeworks
                .Include(csth => csth.Homework)
                .OrderByDescending(csth => csth.SendAt)
                .Where(csth =>
                    csth.ClassStudent.ClassId == clsId &&
                    states.Contains(csth.ClassStudentsToHomeworkState.State) &&
                    csth.Homework.HomeworkState.State != "Deleted" &&
                    csth.ClassStudent.State.State == "Accepted"
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(csth => new StudentToHomeworksForHomeworksSentViewModel
                {
                    HomeworkTitle = csth.Homework.HomeworkTitle,
                    StudentId = csth.ClassStudent.StudentId,
                    StudentToHomeworkId = csth.ClassStudentsToHomeworkId
                }).ToListAsync();

            var homeworksSent = new List<HomeworksSentViewModel>();

            foreach (var classStudentsToHomework in classStudentsToHomeworks)
            {
                var user = await this._userManager.FindByIdAsync(classStudentsToHomework.StudentId);
                homeworksSent.Add(new HomeworksSentViewModel()
                {
                    HomeworkTitle = classStudentsToHomework.HomeworkTitle,
                    StudentToHomeworkId = classStudentsToHomework.StudentToHomeworkId,
                    UserEmail = user.Email,
                    UserImage = user.PicturePath,
                    UserName = user.FullName
                });
            }

            return homeworksSent;
        }

        public async Task<int> GetCountByClassIdentityForSentListAsync(string classIdentity)
        {
            var clsId = await this.getClassIdByClassIdentity(classIdentity);
            var states = await this.getPendingsStates();


            var classStudentsToHomeworksCount = await this._context.ClassStudentsToHomeworks
                .Include(csth => csth.Homework)
                .CountAsync(csth =>
                    csth.ClassStudent.ClassId == clsId &&
                    states.Contains(csth.ClassStudentsToHomeworkState.State) &&
                    csth.Homework.HomeworkState.State != "Deleted" &&
                    csth.ClassStudent.State.State == "Accepted"
                );


            return classStudentsToHomeworksCount;
        }

        public async Task<HomeworkSentCheckViewModel?> GetByClassIdByIdForCheckSentAsync(string classIdentity, int studentToHomeworkId)
        {
            var states = await this.getPendingsStates();
            var clsId = await this.getClassIdByClassIdentity(classIdentity);

            var classStudentsToHomeworks = await this._context.ClassStudentsToHomeworks
                .Where(csth =>
                    csth.ClassStudentsToHomeworkId == studentToHomeworkId &&
                    csth.ClassStudent.ClassId == clsId &&
                    states.Contains(csth.ClassStudentsToHomeworkState.State) &&
                    csth.Homework.HomeworkState.State != "Deleted" &&
                    csth.ClassStudent.State.State == "Accepted"
                )
                .Select(csth => new
                {
                    ClassStudentToHomeworkId = csth.ClassStudentsToHomeworkId,
                    StudentId = csth.ClassStudent.StudentId,
                    HomeworkTitle = csth.Homework.HomeworkTitle,
                    Title = csth.Title,
                    Description = csth.Description,
                    SendAt = csth.SendAt,
                }).SingleOrDefaultAsync();

            if (classStudentsToHomeworks == null)
            {
                return null;
            }


            var user = await this._userManager.FindByIdAsync(classStudentsToHomeworks.StudentId);

            var pictureType = await this._context.TableTypes
                .SingleAsync(pt => pt.Type == "ClassStudentsToHomeworks");


            var pictures = await this._context.Pictures
                .Where(p =>
                    p.TableType == pictureType &&
                    p.TableTypeRecordId == classStudentsToHomeworks.ClassStudentToHomeworkId
                )
                .Select(p => p.PicturePath)
                .ToListAsync();


            var HomeworkSentCheck = new HomeworkSentCheckViewModel()
            {
                Pictures = pictures,
                StudentEmail = user.Email,
                StudentImage = user.PicturePath,
                StudentName = user.FullName,
                StudentToHomeworkId = classStudentsToHomeworks.ClassStudentToHomeworkId,
                Description = classStudentsToHomeworks.Description,
                SendAt = classStudentsToHomeworks.SendAt.ToShamsi(),
                Title = classStudentsToHomeworks.Title,
                HomeworkTitle = classStudentsToHomeworks.HomeworkTitle
            };
            return HomeworkSentCheck;
        }

        public async Task<bool> GetByClassIdentityByIdForChangeStateAsync(string classIdentity, int studentToHomeworkId)
        {
            var states = await this.getPendingsStates();
            var clsId = await this.getClassIdByClassIdentity(classIdentity);

            var classStudentsToHomeworks = await this._context.ClassStudentsToHomeworks
                .Include(csth => csth.ClassStudentsToHomeworkState)
                .Where(csth =>
                    csth.ClassStudentsToHomeworkId == studentToHomeworkId &&
                    csth.ClassStudent.ClassId == clsId &&
                    states.Contains(csth.ClassStudentsToHomeworkState.State) &&
                    csth.Homework.HomeworkState.State != "Deleted" &&
                    csth.ClassStudent.State.State == "Accepted"
                )
                .Select(csth => csth.ClassStudentsToHomeworkId)
                .AnyAsync();

            return classStudentsToHomeworks;
        }

        public async Task ChangeStateByClassIdentityByIdByStateAsync(string classIdentity, int studentToHomeworkId, string state)
        {
            var states = await this.getPendingsStates();
            var clsId = await this.getClassIdByClassIdentity(classIdentity);

            var newState = await this._context.ClassStudentsToHomeworkStates
                .SingleAsync(csths => csths.State == state);

            var classStudentsToHomeworks = await this._context.ClassStudentsToHomeworks
                .SingleAsync(csth =>
                    csth.ClassStudentsToHomeworkId == studentToHomeworkId &&
                    csth.ClassStudent.ClassId == clsId &&
                    states.Contains(csth.ClassStudentsToHomeworkState.State)
                );

            classStudentsToHomeworks.ClassStudentsToHomeworkState = newState;
            classStudentsToHomeworks.ClassStudentHomeworkStateId = newState.ClassStudentsToHomeworkStateId;
            this._context.Update(classStudentsToHomeworks);

        }
    }
}
