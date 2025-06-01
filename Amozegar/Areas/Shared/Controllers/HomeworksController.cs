using Amozegar.Areas.Shared.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Controllers
{
    [Route("Panel/{roleName}/{classId}/Homeworks")]
    public class HomeworksController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public HomeworksController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }


        [Route("{pageNumber}/Show")]
        public async Task<IActionResult> Index(string classId, string roleName, int pageNumber)
        {
            ViewBag.Route = "Homeworks";
            ViewBag.IsTeacher = roleName.ToLowerInvariant() == "teacher" ? true : false;

            var student = await this._userManager.FindByNameAsync(User.Identity.Name);

            var classHomeworks = await this._context.HomeworkRepository
                .GetHomeworksByClassIdentityByStudentIdByPageNumberAsync(classId, student.Id, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, classHomeworks.Count()))
            {
                return this.returnToPaginationView();
            }

            var homeworksCount = await this._context.HomeworkRepository
                .GetHomeworksCountByClassIdentityAsync(classId);

            this.checkNextOrPrevForViewBags(homeworksCount, pageNumber);


            if (!ViewBag.IsTeacher)
            {
                var user = await this._userManager.FindByNameAsync(User.Identity.Name);
                await this._context.UsersViewsRepository
                    .ReadAllHomeworksAsync(user, classId);
            }


            foreach (var classHomework in classHomeworks)
            {
                if (TempData.ContainsKey(classHomework.HomeworkId.ToString()))
                {
                    ModelState.AddModelError("HomeworkId", TempData[classHomework.HomeworkId.ToString()].ToString());
                }
            }

            return View(classHomeworks);
        }



        [Route("{homeworkId}/Details")]
        public async Task<IActionResult> Details(string classId, string roleName, int homeworkId)
        {
            ViewBag.Route = "Homeworks";

            var classStudent = await this._userManager.FindByNameAsync(User.Identity.Name);

            var homeworkDetails = await this._context.HomeworkRepository
                .GetHomeworkWithPicturesByIdAndClassIdentityByStudentIdByIdByNotThisStateAsync(classId, classStudent.Id, homeworkId, "Deleted");
            if (homeworkDetails == null)
            {
                return RedirectToAction("Index", "Homeworks", new { area = "Shared", roleName = roleName, classId = this.classId, pageNumber = 1 });
            }


            return View(homeworkDetails);
        }


    }
}
