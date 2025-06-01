using System.Linq;
using Amozegar.Data;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Amozegar.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<User> _userManager;
        private RoleManager<UserRole> _roleManager;
        private SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            SignInManager<User> signInManager)
        {
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            this._userManager = userManager;
        }
        // Utilities
        private async Task setRoles(RegisterViewModel register)
        {
            var roles = await _roleManager.Roles
                .Where(r => r.Name != "Admin")
                .Select(role => new SelectListItem()
                {
                    Text = role.PersianName,
                    Value = role.Name
                })
                .ToListAsync();
            register.Roles = roles;
        }

        private IActionResult returnToPanel()
        {
            return RedirectToAction("Index", "Home", new { area = "Panel" });
        }


        // Main Methods
        [Route("Account/Register")]
        public async Task<IActionResult> Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return returnToPanel();
            }
            var register = new RegisterViewModel();
            await this.setRoles(register);
            return View(register);
        }


        [HttpPost("Account/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return returnToPanel();
            }
            if (!ModelState.IsValid)
            {
                await this.setRoles(register);
                return View(register);
            }   
            var existingUser = await this._userManager.FindByEmailAsync(register.Email.ToLowerInvariant());
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "ایمیل قبلاً ثبت شده است.");
                await this.setRoles(register);
                return View(register);
            }
            if (!(await _roleManager.RoleExistsAsync(register.SelectedRole) && register.SelectedRole != "Admin"))
            {
                ModelState.AddModelError("Roles", "سطح دسترسی نادرست است");
                await this.setRoles(register);
                return View(register);
            }


            var user = new User
            {
                UserName = register.Email.ToLower(),
                Email = register.Email,
                FullName = register.FullName,
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, register.SelectedRole);

            if (register.UserPicture != null && register.UserPicture.Length > 0)
            {
                string fileName = user.Id + Path.GetExtension(register.UserPicture.FileName);
                await register.UserPicture.SaveImage(fileName, "users");
                user.PicturePath = fileName;
                await this._userManager.UpdateAsync(user);
            }

            return RedirectToAction("Login");
        }


        [Route("Account/Login")]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return returnToPanel();
            }
            return View();
        }

        [HttpPost("Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return returnToPanel();
            }
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = await this._userManager.FindByEmailAsync(login.Email.ToLowerInvariant());
            if (user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات مطابقت ندارند.");
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Email", "ورود ناموفق بود. لطفاً اطلاعات را بررسی کنید.");
                return View(login);
            }
        }

        [Authorize]
        [HttpPost("Account/Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
