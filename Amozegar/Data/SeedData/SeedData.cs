using Amozegar.Models;
using Microsoft.AspNetCore.Identity;

namespace Amozegar.Data.SeedData
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();


            string[] roleNames = { "Admin", "Teacher", "Student" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            // Add Admin
            var adminUser = new User()
            {
                UserName = "admin@gmail.com",
                FullName = "koorosh",
                Email = "admin@gmail.com",
                Date = DateTime.Now,
                PicturePath = "user.webp",
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Koorosh@1387");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(adminUser, roleNames);
                }
            }



            // Add Teacher

            var teacherUser = new User()
            {
                UserName = "teacher@gmail.com",
                FullName = "koorosh Teacher",
                Email = "teacher@gmail.com",
                Date = DateTime.Now,
                PicturePath = "user.webp",
            };

            if (await userManager.FindByEmailAsync(teacherUser.Email) == null)
            {
                var result = await userManager.CreateAsync(teacherUser, "Koorosh@1387");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(teacherUser, new string[] { "Teacher", "Student" });
                }
            }

            // Add Student

            var studentUser = new User()
            {
                UserName = "student@gmail.com",
                FullName = "koorosh Student",
                Email = "student@gmail.com",
                Date = DateTime.Now,
                PicturePath = "user.webp",
            };

            if (await userManager.FindByEmailAsync(studentUser.Email) == null)
            {
                var result = await userManager.CreateAsync(studentUser, "Koorosh@1387");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(studentUser, "Student");
                }
            }

        }
    }
}
