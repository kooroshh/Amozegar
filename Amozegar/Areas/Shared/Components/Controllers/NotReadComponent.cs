using System.Collections.Specialized;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Components.Controllers
{
    public class NotReadComponent : ViewComponent
    {

        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public NotReadComponent(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string type, string classIdentity = "")
        {
            var count = 0;
            var user = await this._userManager.FindByNameAsync(User.Identity.Name);
            switch (type)
            {
                case "Notifications":
                    {
                        count = await this._context.UsersViewsRepository
                            .GetUnreadNotificationsCountByUserIdAsync(user.Id);
                        break;
                    }

                case "Students":
                    {
                        count = await _context.ClassStudentsRepository
                            .GetClassStudentsRequestsCountAsync(classIdentity);
                        break;
                    }

            }
            return View("/Areas/Shared/Components/Views/NotRead.cshtml", count);
        }
    }
}
