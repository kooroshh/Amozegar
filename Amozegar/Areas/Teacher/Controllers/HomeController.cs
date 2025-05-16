using System.Linq;
using Amozegar.Areas.Teacher.Models;
using Amozegar.Data;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Amozegar.Models.CustomAnnotations;
using Amozegar.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amozegar.Areas.Teacher.Controllers
{
    [Route("Panel/Teacher/{classId}")]
    public class HomeController : BaseController
    {

        private IUnitOfWork _context;

        public HomeController(
            IUnitOfWork context
        )
        {
            this._context = context;
        }


        public IActionResult Index(string classId)
        {
            ViewBag.Route = "Dashboard";
            return View();
        }

        // Utilities

        private void setPaginationViewBags(int pageNumber)
        {
            ViewBag.HasNext = false;
            ViewBag.HasPrev = false;
            ViewBag.CurrentPage = pageNumber;
        }

        private void checkNextOrPrevForViewBags(int count, int pageNumber)
        {
            var thisPageCount = DefaultPageCount.Count * pageNumber;

            if (count > thisPageCount)
            {
                ViewBag.HasNext = true;
            }

            if (!(thisPageCount - 10 <= 0))
            {
                ViewBag.HasPrev = true;
            }
        }

        private bool validateUserPageNumber(int pageNumber, int count)
        {
            if (pageNumber != 1 && count <= 0)
            {
                return true;
            }
            return false;
        }

        private IActionResult returnToPaginationView()
        {
            return RedirectToAction(ViewBag.Route, "Home", new { classId = this.classId, pageNumber = 1 });
        }

        // Main Methods

        [Route("Logins-Requests/{pageNumber}")]
        public async Task<IActionResult> LoginsToClass(string classId, int pageNumber)
        {
            ViewBag.Route = "LoginsToClass";

            var students = await this._context.ClassStudentsRepository
                .GetClassStudentsRequestsByClassIdentityByPageNumberAsync(classId, pageNumber);


            this.setPaginationViewBags(pageNumber);
            if (validateUserPageNumber(pageNumber, students.Count()))
            {
                return this.returnToPaginationView();
            }
            var requestsCounts = await this._context.ClassStudentsRepository
                .GetClassStudentsRequestsCountAsync(classId);

            this.checkNextOrPrevForViewBags(requestsCounts, pageNumber);

            return View(students);
        }



        [Route("Notifications/{pageNumber}/List")]
        public async Task<IActionResult> Notifications(string classId, int pageNumber)
        {
            ViewBag.Route = "Notifications";

            var notifications = await this._context.NotificationsRepository
                .GetNotificationsByClassIdentityByPageNumberAsync(classId, pageNumber);


            this.setPaginationViewBags(pageNumber);
            if (validateUserPageNumber(pageNumber, notifications.Count()))
            {
                return this.returnToPaginationView();
            }
            var notificationsCount = await this._context.NotificationsRepository
                .GetNotificationsCountByClassIdentityAsync(classId);

            this.checkNextOrPrevForViewBags(notificationsCount, pageNumber);

            ViewBag.NotificationCount = notificationsCount;
            return View(notifications);
        }



        [Route("Homeworks/{pageNumber}")]
        public async Task<IActionResult> Homeworks(string classId, int pageNumber)
        {
            ViewBag.Route = "Homeworks";
            var classHomeworks = await this._context.HomeworkRepository
                .GetHomeworskByClassIdentityByPageNumberAsync(classId, pageNumber);

            this.setPaginationViewBags(pageNumber);

            if (validateUserPageNumber(pageNumber, classHomeworks.Count()))
            {
                return this.returnToPaginationView();
            }

            var homeworksCount = await this._context.HomeworkRepository
                .GetHomeworksCountByClassIdentityAsync(classId);

            this.checkNextOrPrevForViewBags(homeworksCount, pageNumber);

            ViewBag.HomeworksCount = homeworksCount;

            return View(classHomeworks);
        }


    }
}
