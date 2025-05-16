using Amozegar.Areas.Shared.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Controllers
{
    [Route("Panel/{roleName}/{classId}/{type}/{pageNumber}")]
    public class StudentsController : BaseController
    {
        private IUnitOfWork _context;

        public StudentsController(IUnitOfWork context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index(string roleName, string classId, string type, int pageNumber)
        {
            var students = new List<StudentsListViewModel>();
            var studentsCount = 0;


            ViewBag.CurrentPage = pageNumber;
            ViewBag.Route = type;
            if (type != "Students-List" && roleName.ToLowerInvariant() != "teacher")
            {
                return NotFound();
            }
            switch (type)
            {
                case "Students-List":
                case "Class-Students-List":
                    {
                        ViewData["Title"] = "لیست دانش آموزان کلاس";
                        students
                            .AddRange(
                                await this._context.ClassStudentsRepository
                                    .GetStudentsByClassIdentityByStateByPageNumberAsync(classId, "Accepted", pageNumber)
                            );
                        studentsCount = await this._context.ClassStudentsRepository
                            .GetStudentsCountByClassIdentityByStateAsync(classId, "Accepted");
                        break;

                    }

                case "Banned-Students-List":
                    {
                        ViewData["Title"] = "لیست دانش آموزان مسدود شده کلاس";
                        students
                            .AddRange(
                                await this._context.ClassStudentsRepository
                                    .GetStudentsByClassIdentityByStateByPageNumberAsync(classId, "Banned", pageNumber)
                            );
                        studentsCount = await this._context.ClassStudentsRepository
                            .GetStudentsCountByClassIdentityByStateAsync(classId, "Banned");
                        break;
                    }

                default:
                    return NotFound();

            }

            if (pageNumber != 1 && students.Count() <= 0)
            {
                return RedirectToAction("Index", "Students", new
                {
                    classId = this.classId,
                    roleName= ViewBag.roleName,
                    type = ViewBag.Route,
                    pageNumber = 1
                });
            }


            var thisPageCount = DefaultPageCount.Count * pageNumber;

            if (studentsCount > thisPageCount)
            {
                ViewBag.HasNext = true;
            }

            if (!(thisPageCount - 10 <= 0))
            {
                ViewBag.HasPrev = true;
            }

            ViewBag.Count = studentsCount;
            return View(students);
        }
    }
}
