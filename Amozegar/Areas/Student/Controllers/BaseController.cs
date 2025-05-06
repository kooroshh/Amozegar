using Amozegar.Models.CustomAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Amozegar.Areas.Student.Controllers
{
    [Area("Student")]
    [ValidateClassIdStudent]
    [Authorize(Roles = "Student")]
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
    }
}
