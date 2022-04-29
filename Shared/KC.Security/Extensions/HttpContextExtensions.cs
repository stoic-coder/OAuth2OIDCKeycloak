using System.Globalization;
using KC.Security.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace KC.Security.Extensions;

public static class HttpContextExtensions
{
    public static async Task<bool> IsAccessTokenExpiredAsync(this HttpContext context)
    {
        var expiresAt = await context.GetTokenAsync("expires_at");
        if (expiresAt == null) return false;
        var expiresAtAsDateTimeOffset = 
            DateTimeOffset.Parse(expiresAt, CultureInfo.InvariantCulture);
        return (expiresAtAsDateTimeOffset).ToUniversalTime() < DateTime.UtcNow;
    }

    public static async Task<string?> GetAccessTokenAsync(this HttpContext context)
    {
        string? accessToken = null;
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            accessToken = context.Request.Headers["Authorization"].ToString()
                .Remove(0, 7);
        }
            
        if (!string.IsNullOrEmpty(accessToken)) return accessToken;
        if (context.Request.Query.TryGetValue("access_token", out var _accessToken))
        {
            accessToken = StringHelper.Decrypt(_accessToken);
        }

        if (string.IsNullOrEmpty(accessToken))
        {
            accessToken = await context.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        }

            
        return accessToken;
    }
    
    public static async Task LogoutAsync(this HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

    }
}