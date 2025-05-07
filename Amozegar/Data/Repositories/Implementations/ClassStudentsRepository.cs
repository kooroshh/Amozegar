using Amozegar.Areas.Student.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.Repositories.Interfaces;
using Amozegar.Models;
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

        private async Task<IEnumerable<ClassAndStudentAndClassStudentIdAndState>> getClassStudentsByClassIdentityForRelationshipByStateAsync(string classIdentity, string state)
        {
            var cls = await this.getClassByIdentityForRelationshipAsync(classIdentity);
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
                .ToListAsync();
            return classStudents;
        }



        public async Task<IEnumerable<StudentsListViewModel>> GetStudentsByClassIdentityByStateAsync(string classIdentity, string state)
        {

            var classStudents = await this
                .getClassStudentsByClassIdentityForRelationshipByStateAsync(classIdentity, state);

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

        public async Task<IEnumerable<StudentsListForStudentsViewModel>> GetStudentsByClassIdentityByStateForStudentsAsync(string classIdentity, string state)
        {
            var classStudents = await this
                .getClassStudentsByClassIdentityForRelationshipByStateAsync(classIdentity, state);

            var students = new List<StudentsListForStudentsViewModel>();

            foreach (var student in classStudents)
            {
                var studentsInfo = await this._userManager.FindByIdAsync(student.StudentId);
                students.Add(new StudentsListForStudentsViewModel()
                {
                    StudentFullName = studentsInfo.FullName,
                    StudentPicture = studentsInfo.PicturePath,
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
    }
}
