using Amozegar.Areas.Student.Models;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    [Route("Panel/Student/Classes")]
    public class ClassesController : Controller
    {

        private IUnitOfWork _context;
        private UserManager<User> _userManager;
        private PasswordHasher<ClassRoam> _passwordHasher;

        public ClassesController(
            IUnitOfWork context,
            PasswordHasher<ClassRoam> passwordHasher,
            UserManager<User> userManager
            )
        {
            this._context = context;
            this._passwordHasher = passwordHasher;
            this._userManager = userManager;
        }


        // Utilities

        private IActionResult returnToClassLists()
        {
            return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Student", pageNumber = "1" });
        }

        // Main Methods

        [Route("Add-Class")]
        public IActionResult AddClass()
        {
            return View();
        }

        [HttpPost("Add-Class")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClass(AddClassViewModel addClass)
        {
            if (!ModelState.IsValid)
            {
                return View(addClass);
            }
            var existClass = await this._context.ClassesRepository.GetActiveClassByIdentityAsync(addClass.ClassIdentity);
            if (existClass == null)
            {
                ModelState.AddModelError("ClassIdentity", "چنین کلاسی وجود ندارد. لطفا اطلاعات را دوباره برسی نمایید");
                return View(addClass);
            }
            var result = this._passwordHasher.VerifyHashedPassword(existClass, existClass.ClassPassword, addClass.ClassPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("ClassPassword", "اطلاعات همخوانی ندارند");
                return View(addClass);
            }

            var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            var isNew = await this._context.ClassStudentsRepository
                .GetByCheckStudentIsInClassAsync(user, existClass.ClassId);

            var pendingState = await _context.ClassStudentsStatesRepository.GetStateByNameAsync("Pending");
 

            if (isNew == null)
            {


                var ClassStudent = new ClassStudents()
                {
                    ClassId = existClass.ClassId,
                    StudentId = user.Id,
                    User = user,
                    Class = existClass,
                    State = pendingState,
                    ClassStudentStateId = pendingState.id
                };

                await this._context.ClassStudentsRepository.AddAsync(ClassStudent);
            }
            else // User is in class with deferents state
            {
                var userState = isNew.State;
                if (
                    userState.State == "Accepted" ||
                    userState.State == "Banned" ||
                    userState.State == "Pending"
                    )
                {
                    ModelState.AddModelError("ClassIdentity", "امکان ورود به این کلاس برای شما وجود ندارد");
                    return View(addClass);
                }

                isNew.ClassStudentStateId = pendingState.id;
                isNew.State = pendingState;

                this._context.ClassStudentsRepository.Update(isNew);
            }


            await this._context.SaveChangesAsync();

            return this.returnToClassLists();
        }


        [HttpPost("Delete-Class/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var exisitClass = await this._context.ClassesRepository
                .GetByCheckStudentIsInClassAsync(User.Identity.Name, classId);

            if (exisitClass == null)
            {
                return this.returnToClassLists();
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            var deleteStudent = exisitClass.StudentToClasses
                .Single(stc => stc.StudentId == user.Id && stc.ClassId == exisitClass.ClassId);

            var deleteState = await _context.ClassStudentsStatesRepository.GetStateByNameAsync("Dropped");

            deleteStudent.ClassStudentStateId = deleteState.id;

            deleteStudent.State = deleteState;

            this._context.ClassStudentsRepository.Update(deleteStudent);

            await this._context.SaveChangesAsync();

            return this.returnToClassLists();
        }

    }
}
