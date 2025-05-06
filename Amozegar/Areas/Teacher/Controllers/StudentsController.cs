using Amozegar.Areas.Teacher.Models;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private async Task<ClassStudents?> getStudent(int studentInClassId)
        {
            var studentInClass = await _context.ClassStudentsRepository
                .GetStudentInClassByClassIdentityAndClassStudentIdAsync(studentInClassId, this.classId);

            return studentInClass;
        }

        private async Task<StudentsActionsViewModel> setStudentInClassForModel(ClassStudents studentInClass)
        {
            var user = await this._userManager.FindByIdAsync(studentInClass.StudentId);
            StudentsActionsViewModel studentInClassForModel = new StudentsActionsViewModel()
            {
                StudentInClassId = studentInClass.id,
                StudentName = user.FullName
            };

            return studentInClassForModel;
        }

        private async Task setNewStateForStudentInClass(ClassStudents studentInClass, string state)
        {
            var newStudentState = await this._context.ClassStudentsStatesRepository.GetStateByName(state);
            studentInClass.State = newStudentState;
            studentInClass.ClassStudentStateId = newStudentState.id;
            await this._context.SaveChangesAsync();
        }

        private async Task<IActionResult> doGetActions(int studentInClassId)
        {
            var studentInClass = await this.getStudent(studentInClassId);
            if (studentInClass == null)
            {
                return RedirectToAction("LoginsToClass", "Home", new { area = "Teacher", classId = this.classId });
            }

            StudentsActionsViewModel studentInClassForModel = await this.setStudentInClassForModel(studentInClass);
            return View(studentInClassForModel);
        }

        private async Task<IActionResult> doPostActions(StudentsActionsViewModel action, string newState)
        {
            if (!ModelState.IsValid)
            {
                return View(action);
            }
            var studentInClass = await this.getStudent(action.StudentInClassId);
            if (studentInClass == null)
            {
                return RedirectToAction("LoginsToClass", "Home", new { area = "Teacher", classId = this.classId });
            }

            await this.setNewStateForStudentInClass(studentInClass, newState);


            return RedirectToAction("LoginsToClass", "Home", new { area = "Teacher", classId = this.classId });
        }



        [Route("Accept")]
        public async Task<IActionResult> Accept(string classId, int studentInClassId)
        {
            return await this.doGetActions(studentInClassId);
        }

        [HttpPost("Accept")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(string classId, StudentsActionsViewModel accept)
        {
            return await this.doPostActions(accept, "Accepted");
        }

        [Route("Reject")]
        public async Task<IActionResult> Reject(string classId, int studentInClassId)
        {
            return await this.doGetActions(studentInClassId);
        }

        [HttpPost("Reject")]
        public async Task<IActionResult> Reject(string classId, StudentsActionsViewModel reject)
        {
            return await this.doPostActions(reject, "Rejected");
        }

        [Route("Ban")]
        public async Task<IActionResult> Ban(string classId, int studentInClassId)
        {
            return await this.doGetActions(studentInClassId);
        }

        [HttpPost("Ban")]
        public async Task<IActionResult> Ban(string classId, StudentsActionsViewModel ban)
        {
            return await this.doPostActions(ban, "Banned");
        }

    }
}
