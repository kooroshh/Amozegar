using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/{classId}/Students/{studentInClassId}")]
    public class StudentsController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public StudentsController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        private async Task setNewStateForStudentInClass(ClassStudents studentInClass, string state)
        {
            var newStudentState = await this._context.ClassStudentsStatesRepository.GetStateByNameAsync(state);
            studentInClass.State = newStudentState;
            studentInClass.ClassStudentStateId = newStudentState.id;
            await this._context.SaveChangesAsync();
        }

        // Utilities

        private IActionResult returnToStudents()
        {
            return RedirectToAction("Index", "Students", new { area = "Shared", roleName = "Teacher", classId = this.classId, type = "Class-Students-List", pageNumber = 1 });
        }

        // Main Methods
        private async Task<IActionResult> doPostActions(int studentInClassId, string newState)
        {
            var studentInClass = await _context.ClassStudentsRepository
                .GetStudentInClassByClassIdentityAndClassStudentIdAsync(studentInClassId, this.classId);

            if (studentInClass == null)
            {
                return returnToStudents();
            }

            await this.setNewStateForStudentInClass(studentInClass, newState);


            return returnToStudents();
        }



        [HttpPost("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Accepted");
        }


        [HttpPost("Reject")]
        public async Task<IActionResult> Reject(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Rejected");
        }

        [HttpPost("Ban")]
        public async Task<IActionResult> Ban(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Banned");
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> Remove(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Removed");
        }

        [HttpPost("UnBan")]
        public async Task<IActionResult> UnBan(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Removed");
        }
    }
}
