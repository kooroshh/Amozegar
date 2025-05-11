using System.Formats.Tar;
using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.Repositories.Implementations
{


    public class ClassStudentsRepository : GenericRepository<ClassStudents>, IClassStudentsRepository
    {
        private UserManager<User> _userManager;
        public ClassStudentsRepository(AmozegarContext context, UserManager<User> userManager) : base(context)
        {
            this._userManager = userManager;
        }

        private class ClassIdAndIdentity
        {
            public int ClassId { get; set; }
            public string ClassIdentity { get; set; }
        }

        private class ClassAndStudentAndClassStudentIdAndState
        {
            public int ClassId { get; set; }
            public int ClassStudentId { get; set; }
            public string StudentId { get; set; }
            public string State { get; set; }
        }

        private async Task<ClassIdAndIdentity> getClassByIdentityForRelationshipAsync(string classIdentity)
        {
            var cls = await this._context.Classes
                .Select(c => new ClassIdAndIdentity()
                {
                    ClassId = c.ClassId,
                    ClassIdentity = c.ClassIdentity
                })
                .SingleAsync(c => c.ClassIdentity == classIdentity);
            return cls;
        }

        private async Task<IEnumerable<ClassAndStudentAndClassStudentIdAndState>> getClassStudentsByClassIdentityForRelationshipByStateByPageNumberAsync(string classIdentity, string state, int pageNumber)
        {

            var cls = await this.getClassByIdentityForRelationshipAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var classStudents = await this._context.ClassesStudents
                .Include(cs => cs.State)
                .Select(cs => new ClassAndStudentAndClassStudentIdAndState()
                {
                    ClassId = cs.ClassId,
                    State = cs.State.State,
                    StudentId = cs.StudentId,
                    ClassStudentId = cs.id
                })
                .Where(cs => cs.ClassId == cls.ClassId && cs.State == state)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return classStudents;
        }


        // Working Now
        public async Task<IEnumerable<StudentsListViewModel>> GetStudentsByClassIdentityByStateByPageNumberAsync(string classIdentity, string state, int pageNumber)
        {

            var classStudents = await this
                .getClassStudentsByClassIdentityForRelationshipByStateByPageNumberAsync(classIdentity, state, pageNumber);

            var students = new List<StudentsListViewModel>();

            foreach(var student in classStudents)
            {
                var studentsInfo = await this._userManager.FindByIdAsync(student.StudentId);
                students.Add(new StudentsListViewModel()
                {
                    StudentInClassId = student.ClassStudentId,
                    StudentEmail = studentsInfo.Email,
                    StudentFullName = studentsInfo.FullName,
                    StudentPicture = studentsInfo.PicturePath
                });
            }
            return students;
        }


        public async Task<ClassStudents?> GetByCheckStudentIsInClassAsync(User student, int classId)
        {
            var isNew = await this._context.ClassesStudents
                .Include(cs => cs.State)
                .SingleOrDefaultAsync(cs => cs.ClassId == classId && cs.StudentId == student.Id);
            return isNew;
        }

        public async Task<ClassStudents?> GetStudentInClassByClassIdentityAndClassStudentIdAsync(int studentInClassId, string classId)
        {
            var cls = await this._context.Classes
                .SingleAsync(c => c.ClassIdentity == classId);

            var studentInClass = await _context.ClassesStudents
                .Include(cs => cs.State)
                .SingleOrDefaultAsync(cs => cs.id == studentInClassId && cs.ClassId == cls.ClassId);

            return studentInClass;
        }

        public async Task<int> GetStudentsCountByClassIdentityByStateAsync(string classIdentity, string state)
        {
            var cls = await this.getClassByIdentityForRelationshipAsync(classIdentity);
            var classStudentsCount = await this._context.ClassesStudents
                .Include(cs => cs.State)
                .CountAsync(cs => cs.ClassId == cls.ClassId && cs.State.State == state);
                
            return classStudentsCount;
        }

        public async Task<int> GetClassStudentsRequestsCountAsync(string classIdentity)
        {
            var cls = await this.getClassByIdentityForRelationshipAsync(classIdentity);

            var count = await this._context.ClassesStudents
                .Include(cs => cs.State)
                .CountAsync(cs => cs.ClassId == cls.ClassId && cs.State.State == "Pending");

            return count;
        }

        public async Task<List<AddStudentViewModel>?> GetClassStudentsRequestsByClassIdentityByPageNumberAsync(string classIdentity, int pageNumber)
        {
            var cls = await this.getClassByIdentityForRelationshipAsync(classIdentity);
            int page = pageNumber > 0 ? pageNumber : 0;
            int pageSize = pageNumber > 0 ? DefaultPageCount.Count : 0;

            var students = new List<AddStudentViewModel>();
            var studentsRequests = await _context.ClassesStudents
                .Include(c => c.State)
                .Where(c => c.ClassId == cls.ClassId && c.State.State == "Pending")
                .Skip((page -1) * pageSize)
                .Take(pageSize)
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
    }
}
