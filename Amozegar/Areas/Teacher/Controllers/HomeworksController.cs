using Amozegar.Areas.Shared.Models;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/{classId}/Homeworks")]
    public class HomeworksController : BaseController
    {
        private IUnitOfWork _context;
        private UserManager<User> _userManager;

        public HomeworksController(IUnitOfWork context, UserManager<User> userManager)
        {
            this._context = context;
            this._userManager = userManager;

        }

        // Utilities

        private IActionResult returnToHomeworks()
        {
            return RedirectToAction("Index", "Homeworks", new { area = "Shared", roleName = "Teacher", classId = this.classId, pageNumber = 1 });
        }

        private IActionResult returnToHomeworks(int homeworkId)
        {
            return RedirectToAction("EditHomework", "Homeworks", new { area = "Teacher", classId = classId, homeworkId = homeworkId });
        }

        private IActionResult returnToHomeworksSent()
        {
            return RedirectToAction("HomeworksSent", "Homeworks", new { area = "Teacher", classId = ViewBag.classId, pageNumber = 1 });
        }

        private async Task<IActionResult> doGetChangeActionsForHomeworksAsync(int homeworkId)
        {
            var homework = await this._context.HomeworkRepository
                    .GetHomeworkByClassIdentityByHomeworkIdByNotThisStateForChangeStateAsync(this.classId, homeworkId, "Deleted");

            if (homework == null)
            {
                return this.returnToHomeworks();
            }

            return View(homework);
        }

        private async Task<IActionResult> doPostChnageActionForHomeworksAsync(int homeworkId, ChangeHomeworkStateViewModel change, string shouldBe, string to)
        {
            if (!ModelState.IsValid && change.HomeworkId != homeworkId)
            {
                return this.returnToHomeworks();
            }
            var homework = await this._context.HomeworkRepository
                .GetHomeworkByClassIdentityByHomeworkIdByStateForChangeStateAsync(this.classId, homeworkId, shouldBe);

            if (homework == null)
            {
                var state = "";
                var stateTo = "";

                switch (to)
                {
                    case "Open":
                        {
                            state = "باز کردن";
                            stateTo = "باز";
                            break;
                        }

                    case "Closed":
                        {
                            state = "بستن";
                            stateTo = "بسته";
                            break;
                        }

                    case "Deleted":
                        {
                            state = "حذف";
                            stateTo = "باز";
                            break;
                        }
                }

                ModelState.AddModelError("HomeworkTitle", $"امکان {state} تکلیف {stateTo} نیست.");
                return View(change);
            }

            await this._context.HomeworkRepository.ChangeHomeworkState(homeworkId, to);
            await this._context.SaveChangesAsync();

            return this.returnToHomeworks();
        }

        private async Task<Homework?> getNotDeletedHomeworkByIdAsync(int homeworkId)
        {
            var homework = await this._context.HomeworkRepository
                .GetHomeworkByClassIdentityByIdByNotThisStateAsync(this.classId, homeworkId, "Deleted");

            return homework;
        }

        private async Task<Picture?> getPictureByHomeworkdAndPictureIdAsync(int pictureId, int homeworkId)
        {
            var picture = await this._context.PictureRepository
                .GetPictureByClassIdentityByTypeAndRecordIdAndPictureIdAsync(this.classId, pictureId, homeworkId, "Homeworks");

            return picture;
        }

        private async Task<IActionResult> doGetChangeActionForHomeworkSentAsync(int studentToHomeworkId)
        {
            var StudentToHomework = await this._context.ClassStudentsToHomeworksRepository
                .GetByClassIdentityByIdForChangeStateAsync(this.classId, studentToHomeworkId);

            if (StudentToHomework == null)
            {
                return returnToHomeworksSent();
            }

            return View(StudentToHomework);
        }

        private async Task<IActionResult> doPostChangeActionForHomeworkSentAsync(int studentToHomeworkId, string state, ChangeHomeworkSentViewModel change)
        {

            var StudentToHomework = await this._context.ClassStudentsToHomeworksRepository
                .GetByClassIdentityByIdForChangeStateAsync(this.classId, studentToHomeworkId);

            if (StudentToHomework == null)
            {
                return returnToHomeworksSent();
            }

            if (!ModelState.IsValid || change.StudentToHomeworkId != studentToHomeworkId)
            {
                return View(StudentToHomework);
            }


            await this._context.ClassStudentsToHomeworksRepository
                .ChangeStateByClassIdentityByIdByStateAsync(this.classId, studentToHomeworkId, state);

            await this._context.SaveChangesAsync();

            return returnToHomeworksSent();
        }


        // Pictures Methods


        [Route("{homeworkId}/Delete-Picture/{pictureId}")]
        public async Task<IActionResult> DeletePicture(string classId, int homeworkId, int pictureId)
        {
            var homework = await this.getNotDeletedHomeworkByIdAsync(homeworkId);
            var picture = await this.getPictureByHomeworkdAndPictureIdAsync(pictureId, homeworkId);

            if (homework == null || picture == null)
            {
                return this.returnToHomeworks();
            }

            ViewBag.HomeworkId = homeworkId;

            var pictureModel = new PictureForEditViewModel()
            {
                PictureId = picture.PictureId,
                PicturePath = picture.PicturePath
            };
            return View(pictureModel);
        }

        [HttpPost("{homeworkId}/Delete-Picture/{pictureId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePicture(string classId, int homeworkId, int pictureId, PictureForEditViewModel editPicture)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var homework = await this.getNotDeletedHomeworkByIdAsync(homeworkId);

            var picture = await this.getPictureByHomeworkdAndPictureIdAsync(editPicture.PictureId, homeworkId);

            if (homework == null || picture == null || pictureId != editPicture.PictureId)
            {
                return this.returnToHomeworks();
            }

            var picturePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images",
                "homeworks",
                picture.PicturePath
                );

            await this._context.PictureRepository.DeleteByIdAsync(picture.PictureId);
            await this._context.SaveChangesAsync();

            if (System.IO.File.Exists(picturePath))
            {
                System.IO.File.Delete(picturePath);
            }

            return this.returnToHomeworks(homeworkId);
        }

        [Route("{homeworkId}/Add-Picture")]
        public async Task<IActionResult> AddPicture(string classId, int homeworkId)
        {
            var homework = await this.getNotDeletedHomeworkByIdAsync(homeworkId);

            if (homework == null)
            {
                return this.returnToHomeworks();
            }

            ViewBag.HomeworkId = homeworkId;

            return View();
        }

        [HttpPost("{homeworkId}/Add-Picture")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPicture(string classId, int homeworkId, AddPictureViewModel addPicture)
        {
            if (!ModelState.IsValid)
            {
                return View(addPicture);
            }

            var homework = await this.getNotDeletedHomeworkByIdAsync(homeworkId);

            if (homework == null)
            {
                return this.returnToHomeworks();
            }

            if (addPicture.Pictures != null)
            {
                await addPicture.Pictures.SaveImages(this.classId, homework.HomeworkId, _context, "Homeworks");
            }

            return this.returnToHomeworks(homeworkId);
        }


        // Main Methods

        [Route("Add")]
        public IActionResult AddHomework(string classId)
        {
            return View();
        }

        [HttpPost("Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHomework(string classId, AddOrEditHomeworkViewModel add)
        {

            if (!ModelState.IsValid)
            {
                return View(add);
            }


            var cls = await this._context.ClassesRepository
                .GetByClassIdentityAsync(classId);

            var defaultHomeworkState = await this._context.HomeworkStateRepository
                .GetHomeworkStateByStateAsync("Open");

            var homework = new Homework()
            {
                HomeworkTitle = add.Title,
                HomeworkDescription = add.Description,
                ClassId = cls.ClassId,
                ClassRoam = cls,
                HomeworkState = defaultHomeworkState,
                HomeworkStateId = defaultHomeworkState.HomeworkStateId,
            };

            await this._context.HomeworkRepository
                .AddAsync(homework);
            await this._context.SaveChangesAsync();

            if (add.Pictures != null)
            {
                await add.Pictures.SaveImages(this.classId, homework.HomeworkId, _context, "Homeworks");
            }

            return this.returnToHomeworks();
        }

        [Route("Open/{homeworkId}")]
        public async Task<IActionResult> OpenHomework(string classId, int homeworkId)
        {
            return await this.doGetChangeActionsForHomeworksAsync(homeworkId);
        }

        [HttpPost("Open/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenHomework(string classId, int homeworkId, ChangeHomeworkStateViewModel change)
        {
            return await this.doPostChnageActionForHomeworksAsync(homeworkId, change, "Closed", "Open");
        }

        [Route("Close/{homeworkId}")]
        public async Task<IActionResult> CloseHomework(string classId, int homeworkId)
        {
            return await this.doGetChangeActionsForHomeworksAsync(homeworkId);
        }

        [HttpPost("Close/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseHomework(string classId, int homeworkId, ChangeHomeworkStateViewModel change)
        {
            return await this.doPostChnageActionForHomeworksAsync(homeworkId, change, "Open", "Closed");
        }

        [Route("Delete/{homeworkId}")]
        public async Task<IActionResult> DeleteHomework(string classId, int homeworkId)
        {
            return await this.doGetChangeActionsForHomeworksAsync(homeworkId);
        }

        [HttpPost("Delete/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteHomework(string classId, int homeworkId, ChangeHomeworkStateViewModel change)
        {
            return await this.doPostChnageActionForHomeworksAsync(homeworkId, change, "Closed", "Deleted");
        }


        [Route("Edit/{homeworkId}")]
        public async Task<IActionResult> EditHomework(string classId, int homeworkId)
        {
            var homework = await this.getNotDeletedHomeworkByIdAsync(homeworkId);

            if (homework == null)
            {
                return this.returnToHomeworks();
            }


            var picturesPaths = await this._context.PictureRepository
                .GetPicturesForEditByClassIdentityByTypeAndRecordIdAsync(this.classId, homeworkId, "Homeworks");

            ViewBag.HomeworkId = homeworkId;

            var edit = new AddOrEditHomeworkViewModel()
            {
                Title = homework.HomeworkTitle,
                Description = homework.HomeworkDescription,
                PicturesList = picturesPaths,
            };

            return View(edit);
        }


        [HttpPost("Edit/{homeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNotification(string classId, int homeworkId, AddOrEditHomeworkViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            var homework = await this.getNotDeletedHomeworkByIdAsync(homeworkId);
            if (homework == null)
            {
                return this.returnToHomeworks();
            }

            homework.HomeworkDescription = edit.Description;
            homework.HomeworkTitle = edit.Title;
            this._context.HomeworkRepository.Update(homework);
            await this._context.SaveChangesAsync();

            return this.returnToHomeworks();
        }


        [Route("HomeworksSent/{pageNumber}")]
        public async Task<IActionResult> HomeworksSent(string classId, int pageNumber)
        {
            ViewBag.Route = "HomeworksSent";

            var homeworksSents = await this._context.ClassStudentsToHomeworksRepository
                .GetByClassIdentityByPageNumberForSentListAsync(classId, pageNumber);



            this.setPaginationViewBags(pageNumber);

            if (this.validateUserPageNumber(pageNumber, homeworksSents.Count()))
            {
                return this.returnToHomeworksSent();
            }

            var classStudentsToHomeworksCount = await this._context.ClassStudentsToHomeworksRepository
                .GetCountByClassIdentityForSentListAsync(classId);

            this.checkNextOrPrevForViewBags(classStudentsToHomeworksCount, pageNumber);

            return View(homeworksSents);
        }


        [Route("HomeworksSent/Check/{studentToHomeworkId}")]
        public async Task<IActionResult> CheckHomeworkSent(string classId, int studentToHomeworkId)
        {
            ViewBag.Route = "HomeworksSent";

            var checkSent = await this._context.ClassStudentsToHomeworksRepository
                .GetByClassIdByIdForCheckSentAsync(classId, studentToHomeworkId);

            if (checkSent == null)
            {
                return this.returnToHomeworksSent();
            }

            return View(checkSent);
        }

        [Route("HomeworksSent/Check/Accept/{studentToHomeworkId}")]
        public async Task<IActionResult> AcceptSentHomework(string classId, int studentToHomeworkId)
        {
            return await this.doGetChangeActionForHomeworkSentAsync(studentToHomeworkId);
        }

        [HttpPost("HomeworksSent/Check/Accept/{studentToHomeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptSentHomework(string classId, int studentToHomeworkId, ChangeHomeworkSentViewModel accept)
        {
            return await this.doPostChangeActionForHomeworkSentAsync(studentToHomeworkId, "Accepted", accept);
        }

        [Route("HomeworksSent/Check/Reject/{studentToHomeworkId}")]
        public async Task<IActionResult> RejectSentHomework(string classId, int studentToHomeworkId)
        {
            return await this.doGetChangeActionForHomeworkSentAsync(studentToHomeworkId);
        }

        [HttpPost("HomeworksSent/Check/Reject/{studentToHomeworkId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSentHomework(string classId, int studentToHomeworkId, ChangeHomeworkSentViewModel reject)
        {
            return await this.doPostChangeActionForHomeworkSentAsync(studentToHomeworkId, "Rejected", reject);
        }


    }
}
