using Amozegar.Areas.Admin.Models;
using Amozegar.Data.UnitOfWork;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Amozegar.Areas.Shared.Components.Controllers.ChangeRoleComponents
{
    public class ChangeRolesViewComponent : BaseViewComponent
    {
        private IUnitOfWork _context;

        public ChangeRolesViewComponent(IUnitOfWork context) : base()
        {
            this._context = context;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = await this._context.RolesRepository
                .GetRolesForEditUserAsync();

            return View(this.setViewPath("ChangeRolesComponents", "ChangeRolesComponent.cshtml"), roles);
        }
    }
}
