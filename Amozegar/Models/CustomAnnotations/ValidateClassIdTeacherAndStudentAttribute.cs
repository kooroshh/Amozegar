using System.ComponentModel.DataAnnotations;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Amozegar.Models.CustomAnnotations
{
    public class ValidateClassIdTeacherAndStudentAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var db = context.HttpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;
            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;

            var user = await userManager.FindByNameAsync(context.HttpContext.User.Identity.Name);

            if (user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (context.ActionArguments.TryGetValue("classId", out var classIdObj) && classIdObj is string classId)
            {

                var studentWay = await db.ClassesRepository
                    .IsStudentInClassByClassIdentityAndUserIdAsync(classId, user.Id);

                var teacherWay = await db.ClassesRepository
                    .IsClassForTeacherByClassIdentityAndUserIdAsync(classId, user.Id);


                if (!studentWay && !teacherWay)
                {
                    context.Result = new RedirectToActionResult("Classes", "Home", new { area = "Panel", roleName = "Student" });
                    return;
                }
            }

            await next();
        }

        

    }
}
