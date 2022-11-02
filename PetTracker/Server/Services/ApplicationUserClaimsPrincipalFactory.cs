using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PetTracker.Server.Entities;
using System.Security.Claims;

namespace PetTracker.Server.Repository
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
                          IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity claims = await base.GenerateClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);

            claims.AddClaim(new Claim("full_name", string.Join(" ", user.FirstName, user.LastName)));

            foreach (var roleName in roles)
            {
                claims.AddClaim(new Claim("role", roleName));
            }

            return claims;
        }
    }
}
