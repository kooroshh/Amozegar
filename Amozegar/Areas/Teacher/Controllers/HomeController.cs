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

        private IActionResult returnToPaginationView()
        {
            return RedirectToAction(ViewBag.Route, "Home", new { classId = this.classId, pageNumber = 1, area = "Teacher" });
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
            ViewBag.Route = "ControlNotifications";

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


    }
}
