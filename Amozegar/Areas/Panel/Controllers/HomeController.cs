using System.Security.Claims;
using Amozegar.Areas.Panel.Models;
using Amozegar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Panel.Controllers
{
    [Area("Panel")]
    [Authorize]
    public class HomeController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public HomeController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
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
                string file = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    "users",
                    fileName
                );
                user.PicturePath = fileName;
                using (var stream = new FileStream(file, FileMode.Create))
                {
                    edit.UserPicture.CopyTo(stream);
                }
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

    }
}
