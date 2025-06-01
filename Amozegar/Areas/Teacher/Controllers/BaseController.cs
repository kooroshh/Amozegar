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
    public class BaseController : Amozegar.Controllers.BaseController
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

    }
}
