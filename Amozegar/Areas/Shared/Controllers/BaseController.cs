using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;
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
            ViewBag.HasNext = false;
            ViewBag.HasPrev = false;

        }
    }
}
