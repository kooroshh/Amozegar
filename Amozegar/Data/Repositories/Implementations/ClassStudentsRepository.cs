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



        public async Task<ClassStudents?> GetByCheckStudentIsInClass(User student, int classId)
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
