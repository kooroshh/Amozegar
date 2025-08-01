using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin/Classes")]
    public class ClassesController : BaseController
    {
        private IUnitOfWork _context;

        public ClassesController(IUnitOfWork context)
        {
            this._context = context;
        }

        // Utilities

        private IActionResult RedirectToClasses() => RedirectToAction("Index", "Classes", new { pageNumber = 1 });
        private IActionResult RedirectToShowClass(int classId) => RedirectToAction("ShowClass", "Classes", new { area = "Admin", pageNumber = 1, classId = classId});

        private IActionResult RedirectToClasses(string error)
        {
            TempData["Error"] = error;
            return RedirectToAction("Index", "Classes", new { pageNumber = 1 });
        }

        private IActionResult RedirectToShowClass(int classId, string error)
        {
            TempData["Error"] = error;
            return RedirectToAction("ShowClass", "Classes", new { area = "Admin", pageNumber = 1, classId = classId });
        }

        private async Task setNewStateForStudentInClass(ClassStudents studentInClass, string state)
        {
            var newStudentState = await this._context.ClassStudentsStatesRepository.GetStateByNameAsync(state);
            studentInClass.State = newStudentState;
            studentInClass.ClassStudentStateId = newStudentState.id;
            await this._context.SaveChangesAsync();
        }

        private async Task<IActionResult> doPostActions(int classId, int studentInClassId, string newState, params string[] shouldBe)
        {
            var studentInClass = await _context.ClassStudentsRepository
                .GetStudentInClassByClassIdAndClassStudentIdAsync(studentInClassId, classId);

            if (studentInClass == null)
            {
                return this.RedirectToClasses();
            }

            if (!shouldBe.Contains(studentInClass.State.State))
            {
                return this.RedirectToShowClass(classId, "تغییر وضعیت به این حالت امکان پذیر نمیباشد");
            }

            await this.setNewStateForStudentInClass(studentInClass, newState);


            return this.RedirectToShowClass(classId);
        }


        // Main Methods

        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int pageNumber)
        {
            ViewBag.Route = "Classes";

            var classes = await this._context.ClassesRepository
                .GetClassesByPageNumberAsync(pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, classes.Count()))
            {
                return this.RedirectToClasses();
            }

            var ClassesCount = await this._context.ClassesRepository
                .GetClassesCountAsync();

            this.checkNextOrPrevForViewBags(ClassesCount, pageNumber);


            return View(classes);
        }

        [Route("ShowClass/{classId}/{pageNumber}")]
        public async Task<IActionResult> ShowClass(int classId, int pageNumber)
        {
            ViewBag.Route = "Classes";

            var cls = await this._context.ClassesRepository
                .GetClassByClassIdByPageNumberAsync(classId, pageNumber);

            if (cls == null)
            {
                return RedirectToClasses("چنین کلاسی وجود ندارد");
            }

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, cls.Students.Count()))
            {
                return this.RedirectToShowClass(classId);
            }

            var studentsCount = await this._context.ClassStudentsRepository
                .ClassStudentsByClassIdCount(classId);

            this.checkNextOrPrevForViewBags(studentsCount, pageNumber);

            return View(cls);
        }


        [HttpPost("DeleteClass/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var cls = await this._context.ClassesRepository
                .GetClassByIdByNotTheseStatesAsync(classId, "Deleted");

            if (cls == null)
            {
                return this.RedirectToClasses("همچین کلاسی وجود ندارد");
            }

            var deleteType = await this._context.ClassStateRepository
                .GetClassStateByStateAsync("Deleted");

            cls.CLassStateId = deleteType.id;
            cls.ClassState = deleteType;

            this._context.ClassesRepository.Update(cls);

            await this._context.SaveChangesAsync();


            return this.RedirectToClasses();
        }

        [HttpPost("BanClass/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanClass(int classId)
        {
            var cls = await this._context.ClassesRepository
                .GetClassByIdByNotTheseStatesAsync(classId, "Deleted", "Banned");

            if (cls == null)
            {
                return this.RedirectToClasses("امکان بن کردن فقط برای کلاس های فعال قابل انجام است");
            }

            var banType = await this._context.ClassStateRepository
                .GetClassStateByStateAsync("Banned");

            cls.CLassStateId = banType.id;
            cls.ClassState = banType;

            this._context.ClassesRepository.Update(cls);

            await this._context.SaveChangesAsync();


            return this.RedirectToClasses();
        }

        [HttpPost("UnBanClass/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBanClass(int classId)
        {
            var cls = await this._context.ClassesRepository
                .GetClassByIdByTheseStatesAsync(classId, "Banned");

            if (cls == null)
            {
                return this.RedirectToClasses("رفع مسدودیت فقط برای کلاس های مسدود شده امکان پذیر است");
            }

            var banType = await this._context.ClassStateRepository
                .GetClassStateByStateAsync("Active");

            cls.CLassStateId = banType.id;
            cls.ClassState = banType;

            this._context.ClassesRepository.Update(cls);

            await this._context.SaveChangesAsync();


            return this.RedirectToClasses();
        }


        [HttpPost("ShowClass/{classId}/BanStudent/{studentInClassId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanStudent(int classId, int studentInClassId)
        {
            return await this.doPostActions(classId, studentInClassId, "Banned", "Accepted", "Rejected", "Pending", "Dropped", "Removed");
        }

        [HttpPost("ShowClass/{classId}/RemoveStudent/{studentInClassId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveStudent(int classId, int studentInClassId)
        {
            return await this.doPostActions(classId, studentInClassId, "Removed", "Accepted");
        }

        [HttpPost("ShowClass/{classId}/UnBanStudent/{studentInClassId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBanStudent(int classId, int studentInClassId)
        {
            return await this.doPostActions(classId, studentInClassId, "Removed", "Banned");
        }

        [HttpPost("ShowClass/{classId}/AcceptStudent/{studentInClassId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptStudent(int classId, int studentInClassId)
        {
            return await this.doPostActions(classId, studentInClassId, "Accepted", "Pending");
        }

        [HttpPost("ShowClass/{classId}/RejectedStudent/{studentInClassId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectedStudent(int classId, int studentInClassId)
        {
            return await this.doPostActions(classId, studentInClassId, "Rejected", "Pending");
        }


    }
}
