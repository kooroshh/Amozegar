using System.Security.Claims;
using Amozegar.Areas.Panel.Models;
using Amozegar.Data;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Amozegar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize]
    public class HomeController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmozegarContext _context;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager, AmozegarContext context)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._context = context;
        }

        [Route("Panel")]
        public IActionResult Index()
        {
            return View(); 
        }

        [Route("Panel/Edit")]
        public IActionResult EditInformations()
        {
            return View();
        }

        [HttpPost("Panel/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInformations(EditInformationsViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }
            var user = await this._userManager.FindByNameAsync(User.Identity.Name);

            user.FullName = edit.FullName;
            

            if(edit.UserPicture != null && edit.UserPicture.Length > 0)
            {
                string fileName = user.Id + Path.GetExtension(edit.UserPicture.FileName);
                await edit.UserPicture.SaveImage("users", fileName);
                user.PicturePath = fileName;
            }
            await this._userManager.UpdateAsync(user);

            var claims = await this._userManager.GetClaimsAsync(user);

            var oldClaims = claims
                .Where(c => c.Type == "FullName" || c.Type == "Image")
                .ToList();


            foreach(var item in oldClaims)
            {
                await this._userManager.RemoveClaimAsync(user, item);
            }

            await this._userManager.AddClaimAsync(user, new Claim("FullName", user.FullName));
            await this._userManager.AddClaimAsync(user, new Claim("Image", user.PicturePath));
            await this._signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Home", new { area = "Panel" });
        }

        [Route("Panel/Change-Password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("Panel/Change-Password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var result = await _userManager.ChangePasswordAsync(user, change.Password, change.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Password", "تعویض پسورد موفقیت آمیز نبود. لطفا اطاعات را برسی کرده و دوباره تلاش کنید");
                return View(change);
            }
        }

        [Route("Panel/Classes/{roleName}")]
        public async Task<IActionResult> Classes(string roleName)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (!(await _userManager.IsInRoleAsync(user, roleName)) || roleName == "Admin")
            {
                return RedirectToAction("Index", "Home", new { area = "Panel" });
            }
            ViewBag.Role = roleName;
            IEnumerable<ClassesViewModel> classes = new List<ClassesViewModel>();
            if (roleName == "Teacher")
            {
                classes = await this._context.Classes
                    .Where(classes => classes.TeacherId == user.Id)
                    .Select(c => new ClassesViewModel
                    {
                        ClassId = c.ClassId,
                        ClassImage = c.ClassImage,
                        ClassName = c.ClassName,
                        TeacherName = user.FullName,
                        ClassIdentity = c.ClassIdentity
                    }).ToListAsync();
            }
            else if (roleName == "Student")
            {
            }

                return View(classes);
            
        }

    }
}
