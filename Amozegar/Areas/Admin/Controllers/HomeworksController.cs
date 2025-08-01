using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin/Homeworks")]
    public class HomeworksController : BaseController
    {
        private IUnitOfWork _context;

        public HomeworksController(IUnitOfWork context)
        {
            this._context = context;
        }

        // Utilities

        private IActionResult RedirectToHomeworks() => RedirectToAction("Index", "Homeworks", new { pageNumber = 1 });
        private IActionResult RedirectToCheckStudent(int studentToHomeworkId) => RedirectToAction("CheckStudent", "Homeworks", new { area = "Admin", studentToHomeworkId = studentToHomeworkId});

        private IActionResult RedirectToHomeworks(string error)
        {
            TempData["Error"] = error;
            return RedirectToAction("Index", "Homeworks", new { pageNumber = 1 });
        }

        private async Task<IActionResult> doPostActions(int homeworkId, string to, params string[] shouldBe)
        {
            var homework = await this._context.HomeworkRepository
                .GetHomeworkByIdByThisStatesAsync(homeworkId, shouldBe);

            if (homework == null)
            {
                return this.RedirectToHomeworks("چنین تکلیفی وجود ندارد");
            }

            await this._context.HomeworkRepository.ChangeHomeworkStateAsync(homeworkId, to);
            await this._context.SaveChangesAsync();

            return this.RedirectToHomeworks();
        }

        private async Task<IActionResult> doPostChangeActionForHomeworkSentAsync(int studentToHomeworkId, string state)
        {
            var studentToHomework = await this._context.ClassStudentsToHomeworksRepository
                .GetByIdAsync(studentToHomeworkId);

            if (studentToHomework == null)
            {
                return this.RedirectToHomeworks("چنین تکلیف ارسال شده ای برای بررسی کردن وجود ندارد");
            }

            var currentState = await this._context.ClassStudentsToHomeworksStatesRepository
                .GetStateOfStateByStudentToHomeworkIdAsync(studentToHomeworkId);

            if (currentState == "Rejected" || currentState == "Accepted")
            {
                return this.RedirectToCheckStudent(studentToHomeworkId);
            }


            await this._context.ClassStudentsToHomeworksRepository
                .ChangeStateByIdByStateAsync(studentToHomework, state);

            if (state == "Rejected")
            {
                var classIdentity = await this._context.ClassesRepository
                    .GetClassIdentityByStudentToHomeworkId(studentToHomeworkId);

                await ImageActions.DeleteImages(classIdentity, studentToHomeworkId, "ClassStudentsToHomeworks", this._context);
            }

            await this._context.SaveChangesAsync();

            return this.RedirectToCheckStudent(studentToHomeworkId);
        }



        // Main Methods


        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int pageNumber)
        {
            ViewBag.Route = "Homeworks";


            var homeworks = await this._context.HomeworkRepository
                .GetHomeworksByPageNumberAsync(pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, homeworks.Count()))
            {
                return this.RedirectToHomeworks();
            }

            var HomeworksCount = await this._context.HomeworkRepository
                .GetHomeworksCountAsync();

            this.checkNextOrPrevForViewBags(HomeworksCount, pageNumber);


            return View(homeworks);
        }

        [Route("ShowHomework/{homeworkId}/{pageNumber}")]
        public async Task<IActionResult> ShowHomework(int homeworkId, int pageNumber)
        {
            ViewBag.Route = "Homeworks";

            var homework = await this._context.HomeworkRepository
                .GetHomeworkWithStudentsByIdByPageNumberAsync(homeworkId, pageNumber);

            if (homework == null)
            {
                return this.RedirectToHomeworks("چنین تکلیفی وجود ندارد");
            }

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, homework.Students.Count()))
            {
                return RedirectToAction("ShowHomework", "Homeworks", new { area = "Admin", homeworkId = homeworkId, pageNumber = 1 });
            }

            var studentsCount = await this._context.ClassStudentsRepository
                .ClassStudentsByClassIdCount(homework.ClassId, "Accepted");

            this.checkNextOrPrevForViewBags(studentsCount, pageNumber);

            return View(homework);
        }


        [HttpPost("DeleteHomework/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHomework(int homeworkId)
        {
            return await this.doPostActions(homeworkId, "Deleted", "Closed", "Open");
        }


        [HttpPost("OpenHomework/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenHomework(int homeworkId)
        {
            return await this.doPostActions(homeworkId, "Open", "Closed");
        }


        [HttpPost("CloseHomework/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseHomework(int homeworkId)
        {
            return await this.doPostActions(homeworkId, "Closed", "Open");
        }


        [Route("CheckStudent/{studentToHomeworkId}")]
        public async Task<IActionResult> CheckStudent(int studentToHomeworkId)
        {
            ViewBag.Route = "Homeworks";

            var result = await this._context.ClassStudentsToHomeworksRepository
                .GetByIdForCheckAsync(studentToHomeworkId);
            if (result == null)
            {
                return this.RedirectToHomeworks();
            }

            return View(result);
        }


        [HttpPost("CheckStudent/Accept/{studentToHomeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptSentHomework(string classId, int studentToHomeworkId)
        {
            return await this.doPostChangeActionForHomeworkSentAsync(studentToHomeworkId, "Accepted");
        }



        [HttpPost("CheckStudent/Reject/{studentToHomeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSentHomework(string classId, int studentToHomeworkId)
        {
            return await this.doPostChangeActionForHomeworkSentAsync(studentToHomeworkId, "Rejected");
        }


    }
}
