using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Components.Controllers.ConfirmComponents
{
    public class ConfirmViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(this.setViewPath("ConfirmComponents", "ConfirmComponent.cshtml"));
        }
    }
}
