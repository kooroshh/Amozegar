

using Amozegar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Amozegar.Models.CustomAnnotations
{
    public class ValidateClassIdTeacherAttribute : Attribute, IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var db = context.HttpContext.RequestServices.GetService(typeof(AmozegarContext)) as AmozegarContext;
            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;

            var user = await userManager.FindByNameAsync(context.HttpContext.User.Identity.Name);

            if (user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (context.ActionArguments.TryGetValue("classId", out var classIdObj) && classIdObj is string classId)
            {
                var classExists = db.Classes.Any(c => c.ClassIdentity == classId && c.TeacherId == user.Id);

                if (!classExists)
                {
                    context.Result = new RedirectToActionResult("Classes", "Home", new { area = "Panel", roleName = "Teacher" }); 
                    return;
                }
            }

            await next();
        }
    }
}
