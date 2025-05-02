

using Amozegar.Areas.Panel.Models;
using Amozegar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Models.CustomAnnotations
{
    public class ValidateClassIdStudentAttribute : Attribute, IAsyncActionFilter
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

                var classExists = await db.Classes
                    .Include(c => c.ClassState)
                    .Where(c => c.ClassState.State == "Active" && c.ClassIdentity == classId)
                    .Include(c => c.StudentToClasses)
                    .ThenInclude(cl => cl.State)
                    .AnyAsync(cls => cls.StudentToClasses.
                        Single(stc => stc.StudentId == user.Id).ClassId == cls.ClassId &&
                        cls.StudentToClasses.Single(stc => stc.StudentId == user.Id && stc.ClassId == cls.ClassId).State.State == "Accepted"
                    );

                if (!classExists)
                {
                    context.Result = new RedirectToActionResult("Classes", "Home", new { area = "Panel", roleName = "Student" }); 
                    return;
                }
            }

            await next();
        }
    }
}
