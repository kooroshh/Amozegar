using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Shared.Components.Controllers
{
    public class MobileNavigationComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Areas/Shared/Components/Views/MobileNavigationComponent.cshtml");
        }
    }
}
