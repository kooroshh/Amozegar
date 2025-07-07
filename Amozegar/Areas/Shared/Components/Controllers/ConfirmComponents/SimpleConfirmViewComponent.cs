using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Components.Controllers.ConfirmComponents
{
    public class SimpleConfirmViewComponent : BaseViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(this.setViewPath("ConfirmComponents", "SimpleConfirmComponent.cshtml"));
        }
    }
}
