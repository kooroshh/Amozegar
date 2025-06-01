using System.Security.Claims;
using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class HomeworkRepository : GenericRepository<Homework>, IHomeworkRepository
    {
        public HomeworkRepository(AmozegarContext context) : base(context)
        {
        }

        // Utilities

        private Task<ClassRoam> getClassByClassIdentityAsync(string classIdentity)
        {
            var cls = this._context.Classes
                .SingleAsync(c => c.ClassIdentity == classIdentity);
            return cls;
        }

        private async Task<HomeworkState> getStateByStateAsync(string state)
        {
            var homeworkState = await this._context.HomeworksStates
                .SingleAsync(hs => hs.State == state);
            return homeworkState;
        }

        // Main Methods

        public async Task<IEnumerable<HomeworksViewModel>> GetHomeworksByClassIdentityByStudentIdByPageNumberAsync(string classIdentity, string studentId, int pageNumber)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classStudent = await this._context.ClassesStudents
                .SingleOrDefaultAsync(cs => cs.ClassId == cls.ClassId && cs.StudentId == studentId);

            var homeworks = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .Where(h => h.ClassRoam == cls && h.HomeworkState.State != "Deleted")
                .OrderByDescending(h => h.CreatedAt)
                .Skip((page -1) * pageSize)
                .Take(pageSize)
                .Select(h => new HomeworksViewModel()
                {
                    CreatedAt = h.CreatedAt.ToShamsi(),
                    HomewordTitle = h.HomeworkTitle,
                    HomeworkId = h.HomeworkId,
                    State = h.HomeworkState.PersianState,
                })
                .ToListAsync();

            if (classStudent != null)
            {
                var homeworkIds = homeworks.Select(h => h.HomeworkId).ToList();

                var studentToHomeworks = await _context.ClassStudentsToHomeworks
                    .Include(csth => csth.ClassStudentsToHomeworkState)
                    .Where(csth => csth.ClassStudentId == classStudent.id && homeworkIds.Contains(csth.HomeworkId))
                    .ToListAsync();

                foreach (var homework in homeworks)
                {
                    var stHomework = studentToHomeworks.SingleOrDefault(s => s.HomeworkId == homework.HomeworkId);
                    homework.StudentState = stHomework?.ClassStudentsToHomeworkState?.State ?? "";
                    homework.PersianStudentState = stHomework?.ClassStudentsToHomeworkState?.PersianState ?? "ارسال نشده";
                }
            }

            return homeworks;
        }


        public async Task<IEnumerable<HomeworksViewModel>> GetNotSentHomeworksByClassIdentityByStudentIdByPageNumber(string classIdentity, string studentId, int pageNumber)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classStudent = await this._context.ClassesStudents
                .SingleAsync(cs => cs.ClassId == cls.ClassId && cs.StudentId == studentId);

            var homeworks = await (
                from h in _context.Homeworks
                join csth in _context.ClassStudentsToHomeworks
                    .Include(x => x.ClassStudentsToHomeworkState)
                    on new { h.HomeworkId, StudentId = classStudent.id }
                    equals new { csth.HomeworkId, StudentId = csth.ClassStudentId }
                    into gj
                from csth in gj.DefaultIfEmpty()
                where h.ClassRoam == cls &&
                      h.HomeworkState.State != "Deleted" &&
                      (csth == null || csth.ClassStudentsToHomeworkState == null || csth.ClassStudentsToHomeworkState.State == "Rejected")
                orderby h.CreatedAt descending
                select new HomeworksViewModel
                {
                    CreatedAt = h.CreatedAt.ToShamsi(),
                    HomewordTitle = h.HomeworkTitle,
                    HomeworkId = h.HomeworkId,
                    State = h.HomeworkState.PersianState,
                    StudentState = csth.ClassStudentsToHomeworkState.State ?? "",
                    PersianStudentState = csth.ClassStudentsToHomeworkState.PersianState ?? "ارسال نشده"
                }
            )
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return homeworks;
        }


        public async Task<int> GetHomeworksCountByClassIdentityAsync(string classIdentity)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworks = await this._context.Homeworks
                .CountAsync(h => h.ClassRoam == cls && h.HomeworkState.State != "Deleted");

            return homeworks;
        }

        public async Task<int> GetNotSentHomeworksCountByClassIdentityByStudentIdAsync(string classIdentity, string studentId)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);
            var classStudent = await this._context.ClassesStudents
                .SingleAsync(cs => cs.ClassId == cls.ClassId && cs.StudentId == studentId);

            var count = await (
                from h in _context.Homeworks
                join csth in _context.ClassStudentsToHomeworks
                    .Include(x => x.ClassStudentsToHomeworkState)
                    on new { h.HomeworkId, StudentId = classStudent.id }
                    equals new { csth.HomeworkId, StudentId = csth.ClassStudentId }
                    into gj
                from csth in gj.DefaultIfEmpty()
                where h.ClassRoam == cls &&
                      h.HomeworkState.State != "Deleted" &&
                      (csth == null || csth.ClassStudentsToHomeworkState == null || csth.ClassStudentsToHomeworkState.State == "Rejected")
                select h
            ).CountAsync();

            return count;
        }

        public async Task<ChangeHomeworkViewModel?> GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(string classIdentity, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .Where(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState == homeworkState
                )
                .Select(h => new ChangeHomeworkViewModel()
                {
                    HomeworkId = h.HomeworkId,
                    HomeworkTitle = h.HomeworkTitle
                })
                .SingleOrDefaultAsync();

            return homework;

        }

        public async Task ChangeHomeworkState(int homeworkId, string state)
        {
            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .SingleAsync(h => h.HomeworkId == homeworkId);

            homework.HomeworkState = homeworkState;
            homework.HomeworkStateId = homeworkState.HomeworkStateId;

            this._context.Homeworks.Update(homework);
        }

        public async Task<Homework?> GetHomeworkByClassIdentityByIdByNotThisStateAsync(string classIdentity, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .SingleOrDefaultAsync(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState != homeworkState
                );

            return homework;
        }

        public async Task<HomeworkDetailsViewModel?> GetHomeworkWithPicturesByIdAndClassIdentityByStudentIdByIdByNotThisStateAsync(string classIdentity, string studentId, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);

            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .SingleOrDefaultAsync(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState != homeworkState
                );


            if (homework == null)
            {
                return null;
            }

            var pictureType = await this._context.TableTypes
                .SingleAsync(pt => pt.Type == "Homeworks");


            var pictures = await this._context.Pictures
                .Where(p => p.TableType == pictureType && p.TableTypeRecordId == homework.HomeworkId && p.ClassId == cls.ClassId)
                .Select(p => p.PicturePath)
                .ToListAsync();

            var classStudent = await this._context.ClassesStudents
                .SingleAsync(cs => cs.ClassId == cls.ClassId && cs.StudentId == studentId);

            var studentToHomeworks = await this._context.ClassStudentsToHomeworks
                .Include(csth => csth.ClassStudentsToHomeworkState)
                .SingleOrDefaultAsync(csth => csth.HomeworkId == homework.HomeworkId && csth.ClassStudentId == classStudent.id);

            var detaildHomework = new HomeworkDetailsViewModel()
            {
                CreatedAt = homework.CreatedAt.ToShamsi(),
                HomeworkBody = homework.HomeworkDescription,
                HomeworkTitle = homework.HomeworkTitle,
                HomeworkId = homework.HomeworkId,
                PicturePaths = pictures,
                PersianStudentState = (studentToHomeworks == null) ? "ارسال نشده" : studentToHomeworks.ClassStudentsToHomeworkState.PersianState,
                StudentState = (studentToHomeworks == null) ? "" : studentToHomeworks.ClassStudentsToHomeworkState.State,
            };

            return detaildHomework;
        }

        public async Task<ChangeHomeworkViewModel?> IsHomeworkExistByClassIdentityByIdByStateAsync(string classIdentity, int homeworkId, string state)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);

            var homeworkState = await this.getStateByStateAsync(state);



            var homework = await this._context.Homeworks
                .Include(h => h.HomeworkState)
                .Where(h =>
                    h.HomeworkId == homeworkId &&
                    h.ClassId == cls.ClassId &&
                    h.HomeworkState == homeworkState
                ).Select(h => new ChangeHomeworkViewModel()
                {
                    HomeworkId = h.HomeworkId,
                    HomeworkTitle = h.HomeworkTitle
                })
                .SingleOrDefaultAsync();

            return homework;
        }


    }
}
