using Amozegar.Models.CustomAnnotations;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    [ValidateClassIdTeacher]
    public class BaseController : Controller
    {
        protected string classId;
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.RouteData.Values.TryGetValue("classId", out var classId))
            {
                this.classId = classId.ToString();
            }

            ViewBag.classId = this.classId;

        }


        // Utilities
        protected void setPaginationViewBags(int pageNumber)
        {
            ViewBag.HasNext = false;
            ViewBag.HasPrev = false;
            ViewBag.CurrentPage = pageNumber;
        }

        protected bool validateUserPageNumber(int pageNumber, int count)
        {
            if (pageNumber != 1 && count <= 0)
            {
                return true;
            }
            return false;
        }

        protected void checkNextOrPrevForViewBags(int count, int pageNumber)
        {
            var thisPageCount = DefaultPageCount.Count * pageNumber;

            if (count > thisPageCount)
            {
                ViewBag.HasNext = true;
            }

            if (!(thisPageCount - 10 <= 0))
            {
                ViewBag.HasPrev = true;
            }
            ViewBag.Count = count;

        }

    }
}
