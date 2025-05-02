using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Data.SeedData
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var context = serviceProvider.GetRequiredService<AmozegarContext>();

            string[] roleNames = { "Admin", "Teacher", "Student" };
            string[] roleNamesPersian = { "ادمین", "معلم", "دانش آموز" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new UserRole(
                        roleName,
                        roleNamesPersian[Array.IndexOf(roleNames, roleName)]
                        )
                        );
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

            #region Add ClassStudents States

                string[] StudentsStates = { "Accepted", "Rejected", "Pending", "Dropped", "Banned" };
                string[] StudentsStatesPersian = { "قبول شده", "قبول نشده", "در انتظار تأیید", "ترک کرده", "بن شده" };
                foreach (var state in StudentsStates)
                {
                    if (!await context.ClassesStudentsStates.AnyAsync(css => css.State == state))
                    {
                        await context.AddAsync(new ClassStudentState()
                        {
                            State = state,
                            PersianState = StudentsStatesPersian[Array.IndexOf(StudentsStates, state)]
                        });
                    }
                }
                context.SaveChanges();

            #endregion

            #region

                string[] classStates = { "Active", "Banned", "Deleted" };
                string[] classStatesPersian = { "فعال", "بن شده", "حذف شده" };
                foreach (var state in classStates)
                {
                    if (!await context.ClassesStates.AnyAsync(cs => cs.State == state))
                    {
                        await context.AddAsync(new ClassStates()
                        {
                            State = state,
                            PersianState = classStatesPersian[Array.IndexOf(classStates, state)]
                        });
                    }
                }
                context.SaveChanges();

            #endregion
        }
    }
}
