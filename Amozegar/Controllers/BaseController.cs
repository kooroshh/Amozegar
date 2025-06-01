using Amozegar.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Controllers
{
    public class BaseController : Controller
    {
        protected void setPaginationViewBags(int pageNumber)
        {
            ViewBag.HasNext = false;
            ViewBag.HasPrev = false;
            ViewBag.CurrentPage = pageNumber;
        }

        protected bool validateUserPageNumber(int pageNumber, int count)
        {
            if (pageNumber != 1 && count <= 0)
            {
                return true;
            }
            return false;
        }

        protected void checkNextOrPrevForViewBags(int count, int pageNumber)
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
            ViewBag.Count = count;
        }
    }
}
