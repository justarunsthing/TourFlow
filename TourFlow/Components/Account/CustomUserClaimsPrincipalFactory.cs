using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TourFlow.Client;
using TourFlow.Data;

namespace TourFlow.Components.Account
{
    public class CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> options)
        : UserClaimsPrincipalFactory<ApplicationUser>(userManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
            string profilePictureUrl = $"https://api.dicebear.com/9.x/glass/svg?seed={user.Id}";

            List<Claim> customClaims =
            [
                new Claim(nameof(UserInfo.FirstName), user.FirstName!),
                new Claim(nameof(UserInfo.LastName), user.LastName!),
                new Claim(nameof(UserInfo.ProfilePictureUrl), profilePictureUrl)
            ];

            identity.AddClaims(customClaims);

            return identity;
        }
    }
}