using Amozegar.Areas.Student.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Student.Controllers
{
    [Route("Panel/Student/{classId}/Homeworks")]
    public class HomeworksController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public HomeworksController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        // Utilities

        private IActionResult returnToHomeworks()
        {
            return RedirectToAction("Index", "Homeworks", new { area = "Shared", roleName = "Student", classId = this.classId, pageNumber = 1 });
        }

        // Main Methods


        [Route("NotSendHomeworks/{pageNumber}")]
        public async Task<IActionResult> NotSendHomeworks(string classId, int pageNumber)
        {

            ViewBag.Route = "NotSendHomeworks";

            var student = await this._userManager.FindByNameAsync(User.Identity.Name);

            var classHomeworks = await this._context.HomeworkRepository
                .GetNotSentHomeworksByClassIdentityByStudentIdByPageNumber(classId, student.Id, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, classHomeworks.Count()))
            {
                return RedirectToAction("NotSendHomeworks", "Homeworks", new { area = "Student", classId = classId, pageNumber = 1 });
            }

            var homeworksCount = await this._context.HomeworkRepository
                .GetNotSentHomeworksCountByClassIdentityByStudentIdAsync(classId, student.Id);

            this.checkNextOrPrevForViewBags(homeworksCount, pageNumber);


            //var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            //await this._context.UsersViewsRepository
            //    .ReadAllHomeworksAsync(user, classId);

            return View(classHomeworks);
        }


        [Route("{homeworkId}/SendHomework")]
        public async Task<IActionResult> Send(string classId, int homeworkId)
        {

            var homework = await this._context.HomeworkRepository
                .GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(classId, homeworkId, "Open");

            if (homework == null)
            {
                return this.returnToHomeworks();
            }



            var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            var cls = await this._context.ClassesRepository.GetByClassIdentityAsync(classId);
            var classStudent = await this._context.ClassStudentsRepository
                .GetByCheckStudentIsInClassAsync(user, cls.ClassId);

            var lastClassStudentsToHomeworks = await this._context.ClassStudentsToHomeworksRepository
                .GetByHomeworkIdByClassStudentIdAsync(homeworkId, classStudent.id);

            if (lastClassStudentsToHomeworks != null && lastClassStudentsToHomeworks.ClassStudentsToHomeworkState.State != "Rejected")
            {
                return this.returnToHomeworks();
            }


            var send = new SendHomeworkViewModel()
            {
                HomeworkId = homework.HomeworkId,
                HomeworkTitle = homework.HomeworkTitle,
            };
            return View(send);
        }

        [HttpPost("{homeworkId}/SendHomework")]
        public async Task<IActionResult> Send(string classId, int homeworkId, SendHomeworkViewModel send)
        {

            var homework = await this._context.HomeworkRepository
                .IsHomeworkExistByClassIdentityByIdByStateAsync(classId, homeworkId, "Open");

            if (homework == null)
            {
                return this.returnToHomeworks();
            }

            if (!ModelState.IsValid || send.HomeworkId != homeworkId)
            {
                send.HomeworkId = homework.HomeworkId;
                send.HomeworkTitle = homework.HomeworkTitle;
                return View(send);
            }




            var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            var cls = await this._context.ClassesRepository.GetByClassIdentityAsync(classId);

            var classStudent = await this._context.ClassStudentsRepository
                .GetByCheckStudentIsInClassAsync(user, cls.ClassId);

            var classStudentsToHomeworks = new ClassStudentsToHomework();

            var lastClassStudentsToHomeworks = await this._context.ClassStudentsToHomeworksRepository
                .GetByHomeworkIdByClassStudentIdAsync(homeworkId, classStudent.id);

            if (lastClassStudentsToHomeworks == null)
            {
                var state = await this._context.ClassStudentsToHomeworksStatesRepository
                    .GetClassStudentsToHomeworksStateByStateAsync("Pending");

                classStudentsToHomeworks.HomeworkId = homeworkId;
                classStudentsToHomeworks.Title = send.Title;
                classStudentsToHomeworks.Description = send.Description;
                classStudentsToHomeworks.ClassStudentsToHomeworkState = state;
                classStudentsToHomeworks.ClassStudentHomeworkStateId = state.ClassStudentsToHomeworkStateId;
                classStudentsToHomeworks.ClassStudent = classStudent;
                classStudentsToHomeworks.ClassStudentId = classStudent.id;

                await this._context.ClassStudentsToHomeworksRepository
                    .AddAsync(classStudentsToHomeworks);
            }
            else
            {
                classStudentsToHomeworks = lastClassStudentsToHomeworks;
                if (classStudentsToHomeworks.ClassStudentsToHomeworkState.State != "Rejected")
                {
                    return this.returnToHomeworks();
                }

                var state = await this._context.ClassStudentsToHomeworksStatesRepository
                    .GetClassStudentsToHomeworksStateByStateAsync("Resubmitted");

                classStudentsToHomeworks.Title = send.Title;
                classStudentsToHomeworks.Description = send.Description;
                classStudentsToHomeworks.ClassStudentHomeworkStateId = state.ClassStudentsToHomeworkStateId;

                var imagesPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "notifications",
                    classStudentsToHomeworks.ClassStudentId.ToString()
                );
                if (Directory.Exists(imagesPath))
                {
                    Directory.Delete(imagesPath, recursive: true);
                }

            }

            await this._context.SaveChangesAsync();

            await send.Pictures.SaveImages(
                this.classId,
                classStudentsToHomeworks.ClassStudentsToHomeworkId,
                _context,
                "ClassStudentsToHomeworks"
            );


            return this.returnToHomeworks();
        }


    }
}
