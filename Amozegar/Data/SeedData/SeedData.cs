using Amozegar.Data.UnitOfWork;
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
            var context = serviceProvider.GetRequiredService<IUnitOfWork>();

            #region Add Default User Roles

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

            #endregion

            #region Add Default Users

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

            #endregion

            #region Add ClassStudents States

                string[] StudentsStates = { "Accepted", "Rejected", "Pending", "Dropped", "Banned", "Removed" };
                string[] StudentsStatesPersian = { "قبول شده", "قبول نشده", "در انتظار تأیید", "ترک کرده", "بن شده", "اخراج شده" };
                foreach (var state in StudentsStates)
                {
                    if (!await context.ClassStudentsStatesRepository.AnyAsync(css => css.State == state))
                    {
                        await context.ClassStudentsStatesRepository.AddAsync(new ClassStudentState()
                        {
                            State = state,
                            PersianState = StudentsStatesPersian[Array.IndexOf(StudentsStates, state)]
                        });
                    }
                }

            #endregion

            #region Add ClassState

                string[] classStates = { "Active", "Banned", "Deleted" };
                string[] classStatesPersian = { "فعال", "بن شده", "حذف شده" };
                foreach (var state in classStates)
                {
                    if (!await context.ClassStateRepository.AnyAsync(cs => cs.State == state))
                    {
                        await context.ClassStateRepository.AddAsync(new ClassStates()
                        {
                            State = state,
                            PersianState = classStatesPersian[Array.IndexOf(classStates, state)]
                        });
                    }
                }

            #endregion


            #region Add Table Types

            string[] tableTypes = { "Notifications", "Homeworks", "ClassStudentsToHomeworks" };
            foreach (var type in tableTypes)
            {
                if (!await context.TableTypesRepository.AnyAsync(cs => cs.Type == type))
                {
                    await context.TableTypesRepository.AddAsync(new TableType()
                    {
                        Type = type
                    });
                }
            }

            #endregion


            #region Add Homework States

            string[] homeworkState = { "Closed", "Open", "Deleted" };
            string[] homeworkPersianState = { "بسته شده", "باز", "حذف شده" };
            foreach (var state in homeworkState)
            {
                if (!await context.HomeworkStateRepository.AnyAsync(hs => hs.State == state))
                {
                    await context.HomeworkStateRepository.AddAsync(new HomeworkState()
                    {
                        State = state,
                        PersianState = homeworkPersianState[Array.IndexOf(homeworkState, state)]
                    });
                }
            }

            #endregion

            #region Add Homework Student States

            string[] homeworkStudenState = { "Accepted", "Rejected", "Pending", "Resubmitted" };
            string[] homeworkStudentPersianState = { "قبول شده", "قبول نشده",  "در حال بررسی", "در حال بازبررسی" };
            foreach (var state in homeworkStudenState)
            {
                if (!await context.ClassStudentsToHomeworksStatesRepository.AnyAsync(shs => shs.State == state))
                {
                    await context.ClassStudentsToHomeworksStatesRepository.AddAsync(new ClassStudentsToHomeworkState()
                    {
                        State = state,
                        PersianState = homeworkStudentPersianState[Array.IndexOf(homeworkStudenState, state)]
                    });
                }
            }

            #endregion

            await context.SaveChangesAsync();
        }
    }
}
