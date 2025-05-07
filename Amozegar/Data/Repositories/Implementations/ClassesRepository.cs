using System.Linq;
using Amozegar.Areas.Panel.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
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

        public async Task<List<AddStudentViewModel>?> GetClassStudentsRequestsAsync(string classIdentity)
        {
            var students = new List<AddStudentViewModel>();
            var studentsRequests = await _context.Classes
                .Include(c => c.StudentToClasses)
                .ThenInclude(c => c.State)
                .Where(c => c.ClassIdentity == classIdentity)
                .SelectMany(c => c.StudentToClasses
                    .Where(stc => stc.State.State == "Pending")
                )
                .ToListAsync();

            foreach (var item in studentsRequests)
            {
                var user = await this._userManager.FindByIdAsync(item.StudentId);
                students.Add(new AddStudentViewModel()
                {
                    StudentEmail = user.Email,
                    StudentImage = user.PicturePath,
                    StudentInClassId = item.id,
                    StudentName = user.FullName
                });
            }
            return students;
        }

        public async Task<int> GetClassStudentsRequestsCountAsync(string classIdentity)
        {
            var studentRequestCount = await _context.Classes
                    .Include(c => c.StudentToClasses)
                    .ThenInclude(c => c.State)
                    .Where(c =>
                        c.ClassIdentity == classIdentity &&
                        c.StudentToClasses.Any(stc => stc.State.State == "Pending")
                    )
                    .CountAsync();
            return studentRequestCount;
        }

        public async Task<IEnumerable<ClassesViewModel>> GetStudentsClassesAsync(User user)
        {
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

        public async Task<IEnumerable<ClassesViewModel>> GetTeachersClassesAsync(User user)
        { 
            var classes = await this._dbSet
                .Include(c => c.ClassState)
                .Where(classes => classes.TeacherId == user.Id && classes.ClassState.State == "Active")
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

    }
}
