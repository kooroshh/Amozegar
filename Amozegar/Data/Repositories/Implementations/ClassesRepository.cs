using System.Linq;
using Amozegar.Areas.Panel.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{
    public class ClassesRepository : GenericRepository<ClassRoam>, IClassesRepository
    {
        private UserManager<User> _userManager;
        public ClassesRepository(AmozegarContext context, UserManager<User> userManager) : base(context)
        {
            this._userManager = userManager;
        }

        public async Task<ClassRoam?> GetActiveClassByIdentityAsync(string classIdentity)
        {
            var cls = await this._context.Classes
                .Include(c => c.ClassState)
                .SingleOrDefaultAsync(c => c.ClassIdentity == classIdentity && c.ClassState.State == "Active");

            return cls;
        }

        public async Task<ClassRoam?> GetByCheckStudentIsInClassAsync(string studentName, int classId)
        {
            var user = await _userManager.FindByNameAsync(studentName);
            var exisitClass = await this._context.Classes
                .Include(c => c.ClassState)
                .Include(c => c.StudentToClasses)
                .ThenInclude(cl => cl.State)
                .SingleOrDefaultAsync(
                    cls => cls.ClassId == classId &&
                    cls.ClassState.State == "Active" &&
                    cls.StudentToClasses.Any(
                    stc => stc.StudentId == user.Id && stc.ClassId == classId && stc.State.State == "Accepted"
                ));
            return exisitClass;
        }

        public async Task<ClassRoam?> GetByClassIdentityAsync(string classIdentity)
        {
            return await this._context.Classes.SingleOrDefaultAsync(c => c.ClassIdentity == classIdentity);
        }

        public async Task<ClassRoam?> GetClassByIdAndStateAsync(int classId, string teacherName, string state)
        {
            var user = await _userManager.FindByNameAsync(teacherName);
            var exisitClass = await this._context.Classes
                .Include(c => c.ClassState)
                .SingleOrDefaultAsync(c => c.TeacherId == user.Id && c.ClassId == classId && c.ClassState.State == state);
            return exisitClass;
        }

        public async Task<IEnumerable<ClassesViewModel>> GetStudentsClassesByUserByPageNumberAsync(User user, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classes = await this._context.Classes
                .Include(c => c.ClassState)
                .Include(c => c.Teacher)
                .Include(c => c.StudentToClasses)
                .ThenInclude(cl => cl.State)
                .Where(cls =>
                    cls.ClassState.State == "Active" &&
                    cls.StudentToClasses
                        .Any(stc =>
                            stc.StudentId == user.Id &&
                            stc.ClassId == cls.ClassId &&
                            stc.State.State != "Dropped"
                        )
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClassesViewModel()
                {
                    ClassId = c.ClassId,
                    ClassIdentity = c.ClassIdentity,
                    ClassImage = c.ClassImage,
                    ClassName = c.ClassName,
                    ClassState = c.StudentToClasses
                        .Single(cls => cls.StudentId == user.Id && cls.ClassId == c.ClassId).State.State,
                    ClassStatePersian = c.StudentToClasses
                        .Single(cls => cls.StudentId == user.Id && cls.ClassId == c.ClassId).State.PersianState,
                    TeacherName = c.Teacher.FullName
                })
                .ToListAsync();
            return classes;
        }

        public async Task<int> GetStudentsClassesCountByPageNumberAsync(User user)
        {

            var classesCount = await this._context.Classes
                .Include(c => c.ClassState)
                .Include(c => c.Teacher)
                .Include(c => c.StudentToClasses)
                .ThenInclude(cl => cl.State)
                .Where(cls =>
                    cls.ClassState.State == "Active" &&
                    cls.StudentToClasses
                        .Any(stc =>
                            stc.StudentId == user.Id &&
                            stc.ClassId == cls.ClassId &&
                            stc.State.State != "Dropped"
                        )
                ).CountAsync();
            return classesCount;
        }

        public async Task<IEnumerable<ClassesViewModel>> GetTeachersClassesByUserByPageNumberAsync(User user, int pageNumber)
        {
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classes = await this._dbSet
                .Include(c => c.ClassState)
                .Where(classes => classes.TeacherId == user.Id && classes.ClassState.State == "Active")
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClassesViewModel
                {
                    ClassId = c.ClassId,
                    ClassImage = c.ClassImage,
                    ClassName = c.ClassName,
                    TeacherName = user.FullName,
                    ClassIdentity = c.ClassIdentity
                }).ToListAsync();
            return classes;
        }

        public async Task<int> GetTeachersClassesCountByPageNumberAsync(User user)
        {
            var classesCount = await this._dbSet
                .Include(c => c.ClassState)
                .Where(classes => classes.TeacherId == user.Id && classes.ClassState.State == "Active")
                .CountAsync();
            return classesCount;
        }

        public async Task<bool> IsClassForTeacherByClassIdentityAndUserIdAsync(string classIdentity, string userId)
        {
            var cls = await _context.Classes
                .Include(c => c.ClassState)
                .AnyAsync(c => c.ClassIdentity == classIdentity && c.TeacherId == userId && c.ClassState.State == "Active");
            return cls;
        }

        public async Task<bool> IsStudentInClassByClassIdentityAndUserIdAsync(string classIdentity, string userId)
        {
            var cls = await _context.Classes
                .Include(c => c.ClassState)
                .Where(c => c.ClassState.State == "Active" && c.ClassIdentity == classIdentity)
                .Include(c => c.StudentToClasses)
                .ThenInclude(cl => cl.State)
                .AnyAsync(cls => cls.StudentToClasses
                    .Single(stc =>
                        stc.StudentId == userId
                    ).ClassId == cls.ClassId &&
                    cls.StudentToClasses
                        .Single(stc =>
                            stc.StudentId == userId &&
                            stc.ClassId == cls.ClassId
                        ).State.State == "Accepted"
                );
            return cls;
        }
    }
}
