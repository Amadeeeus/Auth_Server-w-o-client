using Authorization.Core.JWT;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Infrastructure.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IJwtTokenGenerator generator)
    {
        var token = context.Request.Headers["Access"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            try
            {
                var principal = generator.ValidateToken(token);
                context.User = principal;
            }
            catch (SecurityTokenExpiredException)
            {
                var refreshToken = context.Request.Cookies["RefreshToken"];
                if (refreshToken != null)
                {
                    var newAccessToken = generator.GetPrincipalFromExpiredToken(refreshToken);
                    if (newAccessToken != null)
                    {
                        context.Response.Headers.Add("New-Access-Token", newAccessToken.ToString());
                        context.User = generator.ValidateToken(newAccessToken.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Invalid Token");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid Token");
                    return;
                }

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync(ex.Message);
                return;
            }
        }
        await _next(context);
    }
}