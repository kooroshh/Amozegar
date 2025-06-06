﻿using Amozegar.Areas.Teacher.Models;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/Classes")]
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class ClassesController : Controller
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;
        private PasswordHasher<ClassRoam> _passwordHasher;

        public ClassesController(
            IUnitOfWork context,
            UserManager<User> userManager,
            PasswordHasher<ClassRoam> passwordHasher
        )
        {
            this._context = context;
            this._userManager = userManager;
            this._passwordHasher = passwordHasher;
        }

        private async Task<ClassRoam?> isClassExistForTeacher(int classId)
        {
            var existClass = await this._context.ClassesRepository.GetClassByIdAndStateAsync(classId, User.Identity.Name, "Active");
            return existClass;
        }

        // Utilities

        private IActionResult returnToClassLists()
        {
            return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher", pageNumber = "1" });
        }


        // Main Methods

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

            var IsClassExisit = await this._context.ClassesRepository
                .AnyAsync(c => c.ClassIdentity == addClass.ClassIdentity);

            if (IsClassExisit)
            {
                ModelState.AddModelError("ClassIdentity", "کلاسی با این ایدی از قبل ثبت شده است");
                return View(addClass);
            }

            var teacher = await _userManager.FindByNameAsync(User.Identity.Name);
            var activeState = await this._context.ClassStateRepository
                .GetClassStateByStateAsync("Active");

            var newClass = new ClassRoam()
            {
                ClassName = addClass.ClassName,
                TeacherId = teacher.Id,
                Teacher = teacher,
                ClassIdentity = addClass.ClassIdentity,
                ClassState = activeState,
                CLassStateId = activeState.id
            };

            var hashedPassword = _passwordHasher.HashPassword(newClass, addClass.ClassPassword);

            newClass.ClassPassword = hashedPassword;

            await this._context.ClassesRepository.AddAsync(newClass);
            await this._context.SaveChangesAsync();

            if (addClass.ClassImage != null && addClass.ClassImage.Length > 0)
            {
                string fileName = newClass.ClassId + Path.GetExtension(addClass.ClassImage.FileName);
                await addClass.ClassImage.SaveImage(fileName, "classes");
                newClass.ClassImage = fileName;
                this._context.ClassesRepository.Update(newClass);
                await this._context.SaveChangesAsync();
            }

            return RedirectToAction("Classes", "Home", new { area = "Panel", roleName = "Teacher", pageNumber = "1" });
        }

        [HttpPost("Delete-Class/{classId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClass(int classId)
        {

            var existClass = await this.isClassExistForTeacher(classId);

            if (existClass == null)
            {
                return this.returnToClassLists();
            }

            var deleteState = await this._context.ClassStateRepository.GetClassStateByStateAsync("Deleted");

            existClass.CLassStateId = deleteState.id;
            existClass.ClassState = deleteState;
            await this._context.SaveChangesAsync();


            return this.returnToClassLists();
        }


        [Route("Edit-Class/{classId}")]
        public async Task<IActionResult> EditClass(int classId)
        {
            var existClass = await this.isClassExistForTeacher(classId);

            if (existClass == null)
            {
                return this.returnToClassLists();
            }

            var editClass = new EditClassViewModel()
            {
                ClassIdentity = existClass.ClassIdentity,
                ClassName = existClass.ClassName,
                ImagePath = existClass.ClassImage
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

            var existClass = await this.isClassExistForTeacher(classId);

            if (existClass == null)
            {
                return this.returnToClassLists();
            }

            var IsClassExisit = await this._context.ClassesRepository
                .AnyAsync(c => c.ClassIdentity == editClass.ClassIdentity && c.ClassIdentity != existClass.ClassIdentity);

            if (IsClassExisit)
            {
                ModelState.AddModelError("ClassIdentity", "کلاسی با این ایدی از قبل ثبت شده است");
                return View(editClass);
            }


            existClass.ClassName = editClass.ClassName;
            existClass.ClassIdentity = editClass.ClassIdentity;
            
            if (editClass.ClassPassword != null || editClass.NewPassword != null)
            {
                var checkPassword = _passwordHasher.VerifyHashedPassword(existClass, existClass.ClassPassword, (editClass.ClassPassword ?? ""));
                if (checkPassword == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("ClassPassword", "پسورد کلاس اشتباه است");
                    return View(editClass);
                }
                if (editClass.NewPassword == null)
                {
                    ModelState.AddModelError("NewPassword", "لطفا پسورد جدید را وارد کنید !!");
                    return View(editClass);
                }

                string hashedNewPassword = this._passwordHasher.HashPassword(existClass, editClass.NewPassword);
                existClass.ClassPassword = hashedNewPassword;
            }
            await this._context.SaveChangesAsync();
            if (editClass.ClassImage != null && editClass.ClassImage.Length > 0)
            {
                string fileName = existClass.ClassId + Path.GetExtension(editClass.ClassImage.FileName);
                await editClass.ClassImage.SaveImage(fileName, "classes");
                existClass.ClassImage = fileName;
                this._context.ClassesRepository.Update(existClass);
                await this._context.SaveChangesAsync();
            }



            return this.returnToClassLists();
        }



    }
}
