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
                .OrderByDescending(h => h.HomeworkState.State == "Open")
                .ThenByDescending(h => h.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new HomeworksViewModel()
                {
                    CreatedAt = h.CreatedAt.ToShamsi(),
                    HomewordTitle = h.HomeworkTitle,
                    HomeworkId = h.HomeworkId,
                    State = h.HomeworkState.PersianState,
                    HomeworkState = h.HomeworkState.State,
                })
                .ToListAsync();

            if (classStudent != null)
            {
                var homeworkIds = homeworks.Select(h => h.HomeworkId).ToList();

                var studentToHomeworks = await _context.ClassStudentsToHomeworks
                    .Include(csth => csth.ClassStudentsToHomeworkState)
                    .Where(csth => csth.ClassStudentId == classStudent.Id && homeworkIds.Contains(csth.HomeworkId))
                    .ToListAsync();

                foreach (var homework in homeworks)
                {
                    var stHomework = studentToHomeworks.SingleOrDefault(s => s.HomeworkId == homework.HomeworkId);
                    homework.StudentState = stHomework?.ClassStudentsToHomeworkState.State ?? "";
                    homework.PersianStudentState = stHomework?.ClassStudentsToHomeworkState.PersianState ?? "ارسال نشده";


                }
            }

            return homeworks;
        }


        public async Task<IEnumerable<HomeworksViewModel>> GetNotSentHomeworksByClassIdentityByStudentIdByPageNumberAsync(string classIdentity, string studentId, int pageNumber)
        {
            var cls = await this.getClassByClassIdentityAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classStudent = await this._context.ClassesStudents
                .SingleAsync(cs => cs.ClassId == cls.ClassId && cs.StudentId == studentId);

            var homeworks = await (
                from h in _context.Homeworks
                where h.HomeworkState.State == "Open"
                join csth in _context.ClassStudentsToHomeworks
                    .Include(x => x.ClassStudentsToHomeworkState)
                    on new { h.HomeworkId, StudentId = classStudent.Id }
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
                where h.HomeworkState.State == "Open"

                join csth in _context.ClassStudentsToHomeworks
                    .Include(x => x.ClassStudentsToHomeworkState)
                    on new { h.HomeworkId, StudentId = classStudent.Id }
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

        public async Task ChangeHomeworkStateAsync(int homeworkId, string state)
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
                .SingleOrDefaultAsync(csth => csth.HomeworkId == homework.HomeworkId && csth.ClassStudentId == classStudent.Id);

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

        public async Task<IEnumerable<Areas.Admin.Models.HomeworksViewModel>> GetHomeworksByPageNumberAsync(int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var homewokrs = await this._context.Homeworks
                .Where(h => h.HomeworkState.State != "Deleted")
                .OrderByDescending(h => h.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(h => new Areas.Admin.Models.HomeworksViewModel()
                {
                    HomeworkId = h.HomeworkId,
                    State = h.HomeworkState.State,
                    ClassIdentity = h.ClassRoam.ClassIdentity,
                    CreatedAt = h.CreatedAt.ToShamsi(),
                    HomeworkTitle = h.HomeworkTitle,
                    PersianState = h.HomeworkState.PersianState
                })
                .ToListAsync();

            return homewokrs;
        }

        public async Task<int> GetHomeworksCountAsync()
        {
            var count = await this._context.Homeworks
                .CountAsync(h => h.HomeworkState.State != "Deleted");

            return count;
        }

        public async Task<Homework?> GetHomeworkByIdByNotThisStatesAsync(int homeworkId, params string[] states)
        {
            var homework = await this._context.Homeworks
                .SingleOrDefaultAsync(h => h.HomeworkId == homeworkId && !states.Contains(h.HomeworkState.State));

            return homework;
        }

        public async Task<Homework?> GetHomeworkByIdByThisStatesAsync(int homeworkId, params string[] states)
        {
            var homework = await this._context.Homeworks
                .SingleOrDefaultAsync(h => h.HomeworkId == homeworkId && states.Contains(h.HomeworkState.State));

            return homework;
        }

        public async Task<Areas.Admin.Models.HomeworkViewModel?> GetHomeworkWithStudentsByIdByPageNumberAsync(int homeworkId, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;


            var homework = await this._context.Homeworks
                .Where(h => h.HomeworkId == homeworkId && h.HomeworkState.State != "Deleted")
                .Select(h => new Areas.Admin.Models.HomeworkViewModel()
                {
                    HomeworkId = h.HomeworkId,
                    HomeworlTitle = h.HomeworkTitle,
                    HomeworkBody = h.HomeworkDescription,
                    CreatedAt = h.CreatedAt.ToShamsi(),
                    HomeworkState = h.HomeworkState.PersianState,
                    State = h.HomeworkState.State,
                    ClassIdentity = h.ClassRoam.ClassIdentity,
                    ClassId = h.ClassId,
                })
                .SingleOrDefaultAsync();


            if (homework == null)
            {
                return null;
            }


            var pictureType = await this._context.TableTypes
                .SingleAsync(pt => pt.Type == "Homeworks");

            homework.PicturesPath = await this._context.Pictures
            .Where(p => p.TableType == pictureType && p.TableTypeRecordId == homework.HomeworkId)
            .Select(p => p.PicturePath)
            .ToListAsync();



            var students = this._context.ClassesStudents
                .Where(cs => cs.ClassId == homework.ClassId && cs.State.State == "Accepted")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(cs => new Areas.Admin.Models.UserViewModelForHomework
                {
                    StudentName = cs.User.FullName,
                    PicturePath = cs.User.PicturePath,
                    StudentStatus = this._context.ClassStudentsToHomeworks
                        .Where(hw => hw.ClassStudentId == cs.Id && hw.HomeworkId == homeworkId)
                        .Select(hw => hw.ClassStudentsToHomeworkState.PersianState)
                        .FirstOrDefault() ?? "ارسال نشده",
                    IsSent = this._context.ClassStudentsToHomeworks
                        .Any(hw => hw.ClassStudentId == cs.Id && hw.HomeworkId == homeworkId && hw.ClassStudentsToHomeworkState.State != "Rejected"),
                    StudentToHomeworkId = this._context.ClassStudentsToHomeworks
                        .SingleOrDefault(hw => hw.ClassStudentId == cs.Id && hw.HomeworkId == homeworkId)
                        .ClassStudentsToHomeworkId
                })
                .ToList();

            homework.Students = students;



            return homework;

        }
    }
}
