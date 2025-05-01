using Amozegar.Areas.Teacher.Models;
using Amozegar.Data;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    [Route("Panel/Teacher/Classes")]
    public class ClassesController : Controller
    {
        private AmozegarContext _context;
        private UserManager<User> _userManager;
        private PasswordHasher<ClassRoam> _passwordHasher;

        public ClassesController(
            AmozegarContext context,
            UserManager<User> userManager,
            PasswordHasher<ClassRoam> passwordHasher
        )
        {
            this._context = context;
            this._userManager = userManager;
            this._passwordHasher = passwordHasher;
        }

        [Route("Add-Class")]
        public IActionResult AddClass()
        {
            return View();
        }


        [HttpPost("Add-Class")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClass(AddClassViewModel addClass)
        {
            if (!ModelState.IsValid)
            {
                return View(addClass);
            }

            var IsClassExisit = await this._context.Classes.AnyAsync(c => c.ClassIdentity == addClass.ClassIdentity);

            if (IsClassExisit)
            {
                ModelState.AddModelError("ClassIdentity", "کلاسی با این ایدی از قبل ثبت شده است");
                return View(addClass);
            }

            var teacher = await _userManager.FindByNameAsync(User.Identity.Name);

            var newClass = new ClassRoam()
            {
                ClassName = addClass.ClassName,
                TeacherId = teacher.Id,
                Teacher = teacher,
                ClassIdentity = addClass.ClassIdentity,
            };

            var hashedPassword = _passwordHasher.HashPassword(newClass, addClass.ClassPassword);

            newClass.ClassPassword = hashedPassword;

            await this._context.AddAsync(newClass);
            this._context.SaveChanges();

            if (addClass.ClassImage != null && addClass.ClassImage.Length > 0)
            {
                string fileName = newClass.ClassId + Path.GetExtension(addClass.ClassImage.FileName);
                await addClass.ClassImage.SaveImage("classes", fileName);
                newClass.ClassImage = fileName;
                this._context.Classes.Update(newClass);
                this._context.SaveChanges();
            }

            return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher" });
        }


        [Route("Delete-Class/{classId}")]
        public async Task<IActionResult> DeleteClass(int classId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var exisitClass = await this._context.Classes
                .SingleOrDefaultAsync(c => c.TeacherId == user.Id && c.ClassId == classId);
            if(exisitClass == null)
            {
                return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher" });
            }
            var classModel = new DeleteClassViewModel()
            {
                ClassName = exisitClass.ClassName,
                ClassId = exisitClass.ClassId
            };
            return View(classModel);
        }

        [HttpPost("Delete-Class/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClass(DeleteClassViewModel deleteClass)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var exisitClass = await this._context.Classes
                .SingleOrDefaultAsync(c => c.TeacherId == user.Id && c.ClassId == deleteClass.ClassId);
            if (exisitClass == null)
            {
                return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher" });
            }

            return Content($"Deleted {deleteClass.ClassId}");
        }


        [Route("Edit-Class/{classId}")]
        public async Task<IActionResult> EditClass(int classId)
        {
            var user = await this._userManager.FindByNameAsync(User.Identity.Name);

            var exisitClass = await this._context.Classes
                .SingleOrDefaultAsync(c => c.TeacherId == user.Id && c.ClassId == classId);
            if (exisitClass == null)
            {
                return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher" });
            }

            var editClass = new EditClassViewModel()
            {
                ClassIdentity = exisitClass.ClassIdentity,
                ClassName = exisitClass.ClassName,
                ImagePath = exisitClass.ClassImage
            };
            return View(editClass);
        }

        [HttpPost("Edit-Class/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditClass(int classId, EditClassViewModel editClass)
        {
            if (!ModelState.IsValid)
            {
                return View(editClass);
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var exisitClass = await this._context.Classes
                .SingleOrDefaultAsync(c => c.TeacherId == user.Id && c.ClassId == classId);
            if (exisitClass == null)
            {
                return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher" });
            }

            var IsClassExisit = await this._context.Classes.AnyAsync(c => c.ClassIdentity == editClass.ClassIdentity && c.ClassIdentity != exisitClass.ClassIdentity);

            if (IsClassExisit)
            {
                ModelState.AddModelError("ClassIdentity", "کلاسی با این ایدی از قبل ثبت شده است");
                return View(editClass);
            }


            exisitClass.ClassName = editClass.ClassName;
            exisitClass.ClassIdentity = editClass.ClassIdentity;
            
            if (editClass.ClassPassword != null)
            {
                string hashedPassword = this._passwordHasher.HashPassword(exisitClass, editClass.ClassPassword);
                exisitClass.ClassPassword = hashedPassword;
            }
            this._context.SaveChanges();
            if (editClass.ClassImage != null && editClass.ClassImage.Length > 0)
            {
                string fileName = exisitClass.ClassId + Path.GetExtension(editClass.ClassImage.FileName);
                await editClass.ClassImage.SaveImage("classes", fileName);
                exisitClass.ClassImage = fileName;
                this._context.Classes.Update(exisitClass);
                this._context.SaveChanges();
            }



            return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher" });
        }



    }
}
