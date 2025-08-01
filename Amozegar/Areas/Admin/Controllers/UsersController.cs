using Amozegar.Areas.Admin.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Admin.Controllers
{
    [Route("Panel/Admin/Users")]
    public class UsersController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UsersController(IUnitOfWork context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = context;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        // Utilities

        public IActionResult RedirectToUsers() => RedirectToAction("Index", "Users", new { area = "Admin", pageNumber = 1 });
        public IActionResult RedirectToUsers(string error)
        {
            TempData["Error"] = error;
            return this.RedirectToUsers();
        }


        // Main Methods

        [Route("{pageNumber}")]
        public async Task<IActionResult> Index(int pageNumber)
        {
            ViewBag.Route = "Users";

            var users = await this._context.UsersRepository
                .GetUsersByPageNumberAsync(pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, users.Count()))
            {
                return this.RedirectToUsers();
            }

            var usersCount = await this._context.UsersRepository
                .GetUsersCount();

            this.checkNextOrPrevForViewBags(usersCount, pageNumber);

            return View(users);
        }

        [HttpPost("BanUser/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToUsers();
            }

            var user = await this._userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToUsers();
            }

            if (user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                return this.RedirectToUsers();
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            return this.RedirectToUsers();
        }

        [HttpPost("UnBan/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnBan(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return this.RedirectToUsers();
            }

            var user = await this._userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToUsers();
            }

            if (user.LockoutEnd == null && user.LockoutEnd < DateTimeOffset.UtcNow)
            {
                return this.RedirectToUsers();
            }

            await _userManager.SetLockoutEndDateAsync(user, null);

            return this.RedirectToUsers();
        }

        [HttpPost("UpdateUserRoles/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserRoles(string userId, EditRoleViewModel roles)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                string errorHtml = "<ul>";
                foreach (var error in errors)
                {
                    errorHtml += $"<li>{error}</li>";
                }
                errorHtml += "</ul>";

                return this.RedirectToUsers(errorHtml);
            }

            var user = await this._userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return this.RedirectToUsers("چنین کاربری وجود ندارد");
            }

            var allRoles = await this._context.RolesRepository
                .GetRolesAsync();

            if (roles.NewRoles.Any(r => !allRoles.Any(ar => ar.Id == r) ))
            {
                return this.RedirectToUsers("نقش انتخابی باید از بین نقش های موجود باشد");
            }



            foreach (var role in allRoles)
            {
                if (roles.NewRoles.Contains(role.Id) && !(await this._userManager.IsInRoleAsync(user, role.Name)))
                {
                    await this._userManager.AddToRoleAsync(user, role.Name);
                    continue;
                }
                else if (!roles.NewRoles.Contains(role.Id) && await this._userManager.IsInRoleAsync(user, role.Name))
                {
                    await this._userManager.RemoveFromRoleAsync(user, role.Name);
                    continue;
                }
            }

            return this.RedirectToUsers();
        }


    }
}
