using Amozegar.Areas.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Components.Controllers.ConfirmComponents
{
    public class ConfirmImagesViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke(string error = "")
        {
            ViewBag.Error = error;
            var viewModel = new AddPictureViewModel();
            return View(this.setViewPath("ConfirmComponents", "ConfirmImagesComponent.cshtml"), viewModel);
        }
    }
}
