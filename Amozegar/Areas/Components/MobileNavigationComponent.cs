using Microsoft.AspNetCore.Mvc;

namespace Amozegar.Areas.Components
{
    public class MobileNavigationComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Areas/Shared/Components/MobileNavigation/MobileNavigationComponent.cshtml");
        }
    }
}
