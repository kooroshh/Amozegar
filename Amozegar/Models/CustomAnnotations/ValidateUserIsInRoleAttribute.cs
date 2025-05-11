using Amozegar.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Models.CustomAnnotations
{
    public class ValidateUserIsInRoleAttribute : Attribute, IAsyncActionFilter 
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userManager = context.HttpContext.RequestServices.GetService(typeof(UserManager<User>)) as UserManager<User>;

            var user = await userManager.FindByNameAsync(context.HttpContext.User.Identity.Name);

            if (user == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (context.ActionArguments.TryGetValue("roleName", out var roleNameObj) && roleNameObj is string roleName)
            {
                if (!await userManager.IsInRoleAsync(user, roleName))
                {
                    context.Result = new RedirectToActionResult("Index", "Home", new { area = "Panel" });
                    return;
                }
            }

            await next();
        }
    }
}
