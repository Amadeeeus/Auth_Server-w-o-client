using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Core.JWT;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(string email, string id);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    string GenerateRefreshToken(string email);
    ClaimsPrincipal ValidateToken(string token);
}