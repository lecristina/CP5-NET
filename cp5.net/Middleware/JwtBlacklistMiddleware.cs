using System.IdentityModel.Tokens.Jwt;
using Cp5.Net.Services;

namespace Cp5.Net.Middleware;

public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    public JwtBlacklistMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklist)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var jti = context.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (!string.IsNullOrEmpty(jti))
            {
                if (await blacklist.IsBlacklistedAsync(jti))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token inválido (blacklist)");
                    return;
                }
            }
        }
        await _next(context);
    }
}


