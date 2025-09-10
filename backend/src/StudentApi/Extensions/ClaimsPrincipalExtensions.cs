using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StudentApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int? GetUserId(this ClaimsPrincipal user)
    {
       
        var sub = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
               ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(sub, out var id) ? id : null;
    }
}
