using Amozegar.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Amozegar.Factory
{
    public class MyClaimsTransformer : IClaimsTransformation
    {
        private readonly UserManager<User> _userManager;

        public MyClaimsTransformer(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = (ClaimsIdentity)principal.Identity;

            var user = await _userManager.GetUserAsync(principal);
            if (user == null) return principal;

            var currentRoleClaims = identity.Claims
                .Where(c => c.Type == identity.RoleClaimType)
                .ToList();

            foreach (var roleClaim in currentRoleClaims)
            {
                identity.RemoveClaim(roleClaim);
            }

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(identity.RoleClaimType, role));
            }

            return principal;
        }
    }
}
