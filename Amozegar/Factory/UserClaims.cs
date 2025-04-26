using System.Security.Claims;
using Amozegar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Amozegar.Factory
{
    public class UserClaims : UserClaimsPrincipalFactory<User>
    {
        public UserClaims(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor){}

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim("FullName", user.FullName));
            identity.AddClaim(new Claim("Image", user.PicturePath));

            return identity;
        }
    }
}
