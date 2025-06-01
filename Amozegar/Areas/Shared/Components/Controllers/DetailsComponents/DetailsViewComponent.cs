using System.Security.Claims;
using Amozegar.Areas.Shared.Components.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Areas.Shared.Components.Controllers.DetailsComponent
{
    public class DetailsViewComponent : BaseViewComponent
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public DetailsViewComponent(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string classIdentity = "")
        {
            var detailsViewModel = new DetailsComponentViewModel();

            if (string.IsNullOrEmpty(classIdentity))
            {
                detailsViewModel.UserFullName = HttpContext.User.FindFirstValue("FullName") ?? "";
                detailsViewModel.ImagePath = HttpContext.User.FindFirstValue("Image") ?? "";
                detailsViewModel.UserName = User.Identity?.Name;
            }
            else
            {
                var currentClass = await _context.ClassesRepository.GetByClassIdentityAsync(classIdentity);
                var teacher = await _userManager.FindByIdAsync(currentClass.TeacherId);

                detailsViewModel.UserFullName = teacher.FullName;
                detailsViewModel.ImagePath = currentClass.ClassImage;
                detailsViewModel.ClassIdentity = currentClass.ClassIdentity;
                detailsViewModel.ClassName = currentClass.ClassName;
            }

            return View(this.setViewPath("DetailsComponents", "DetailsComponent.cshtml"), detailsViewModel);
        }
    }
}
