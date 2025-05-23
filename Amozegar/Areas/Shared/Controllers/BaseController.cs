using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Amozegar.Areas.Shared.Controllers
{
    [Area("Shared")]
    [Authorize(Roles = "Teacher, Student")]
    [ValidateClassIdTeacherAndStudent]
    [ValidateUserIsInRole]
    public class BaseController : Controller
    {
        protected string? classId;
        protected string? roleName;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (context.RouteData.Values.TryGetValue("classId", out var classId))
            {
                this.classId = classId?.ToString();
            }

            if (context.RouteData.Values.TryGetValue("roleName", out var roleName))
            {
                this.roleName = roleName?.ToString();
            }


            ViewBag.classId = this.classId;
            ViewBag.roleName = this.roleName;
        }


        // Utilities

        protected IActionResult returnToPaginationView(string type = "")
        {
            return RedirectToAction("Index", ViewBag.Route, new { area = "Shared", roleName = ViewBag.roleName, classId = ViewBag.classId, pageNumber = 1, type = type });
        }

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
        }

    }
}
