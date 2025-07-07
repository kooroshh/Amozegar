using Amozegar.Areas.Shared.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Controllers
{
    [Route("Panel/{roleName}/{classId}/Exams")]
    public class ExamsController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public ExamsController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }


        [Route("{pageNumber}/Show/{type?}")]
        public async Task<IActionResult> Index(string classId, string roleName, int pageNumber, string type = "")
        {
            IEnumerable<ExamsViewModel> exams = new List<ExamsViewModel>();
            int examsCount;

            if (ViewBag.IsTeacher)
            {
                if (type != "")
                {
                    return NotFound();
                }
                ViewBag.Route = "Exams";
                exams = await this._context.ExamRepository
                    .GetByClassIdentityByPageNumberAsync(classId, pageNumber);
                examsCount = await this._context.ExamRepository
                    .GetCountByClassIdentityAsync(classId);
            }
            else
            {
                switch (type)
                {
                    case "Ongoing":
                        {
                            exams = await this._context.ExamRepository
                                .GetByClassIdentityByPageNumberByStatesAsync(classId, pageNumber, "Scheduled", "Ongoing");
                            ViewBag.Route = "Exams";

                            examsCount = await this._context.ExamRepository
                                .GetCountByClassIdentityByStatesAsync(classId, "Scheduled", "Ongoing");

                            break;
                        }

                    case "Completed":
                        {
                            ViewBag.Route = "CompletedExams";
                            exams = await this._context.ExamRepository
                                .GetByClassIdentityByPageNumberByStatesAsync(classId, pageNumber, "Completed");

                            examsCount = await this._context.ExamRepository
                                .GetCountByClassIdentityByStatesAsync(classId, "Completed");

                            break;
                        }

                    default:
                        {
                            return NotFound();
                        }
                }

            }

            if (TempData["Error"] != null)
            {
                ModelState.AddModelError("ExamId", TempData["Error"].ToString());
            }

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, exams.Count()))
            {
                return this.returnToPaginationView();
            }           


            this.checkNextOrPrevForViewBags(examsCount, pageNumber);


            return View(exams);
        }

    }
}
