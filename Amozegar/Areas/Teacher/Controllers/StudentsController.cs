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



        // Utilities

        private IActionResult returnToStudents()
        {
            return RedirectToAction("Index", "Students", new { area = "Shared", roleName = "Teacher", classId = this.classId, type = "Class-Students-List", pageNumber = 1 });
        }

        private async Task setNewStateForStudentInClass(ClassStudents studentInClass, string state)
        {
            var newStudentState = await this._context.ClassStudentsStatesRepository.GetStateByNameAsync(state);
            studentInClass.State = newStudentState;
            studentInClass.ClassStudentStateId = newStudentState.id;
            await this._context.SaveChangesAsync();
        }

        private async Task<IActionResult> doPostActions(int studentInClassId, string newState, params string[] shouldBe)
        {
            var studentInClass = await _context.ClassStudentsRepository
                .GetStudentInClassByClassIdentityAndClassStudentIdAsync(studentInClassId, this.classId);

            if (studentInClass == null)
            {
                return this.returnToStudents();
            }

            if (!shouldBe.Contains(studentInClass.State.State))
            {
                return this.returnToStudents();
            }

            await this.setNewStateForStudentInClass(studentInClass, newState);


            return returnToStudents();
        }

        // Main Methods




        [HttpPost("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Accepted", "Pending");
        }


        [HttpPost("Reject")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Rejected", "Pending");
        }

        [HttpPost("Ban")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ban(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Banned", "Accepted");
        }

        [HttpPost("Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Removed", "Accepted");
        }

        [HttpPost("UnBan")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBan(string classId, int studentInClassId)
        {
            return await this.doPostActions(studentInClassId, "Removed", "Banned");
        }
    }
}
