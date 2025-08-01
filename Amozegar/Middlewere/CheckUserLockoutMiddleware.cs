using Amozegar.Models;
using Microsoft.AspNetCore.Identity;

namespace Amozegar.Middlewere
{
    public class CheckUserLockoutMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckUserLockoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
                var signInManager = context.RequestServices.GetRequiredService<SignInManager<User>>();

                var user = await userManager.GetUserAsync(context.User);
                if (user != null && await userManager.IsLockedOutAsync(user))
                {
                    await signInManager.SignOutAsync();
                    context.Response.Redirect("/Account/Login");
                    return;
                }
            }

            await _next(context);
        }
    }
}
