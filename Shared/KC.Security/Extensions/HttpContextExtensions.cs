// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="HttpContextExtensions.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Globalization;
using KC.Security.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace KC.Security.Extensions;

/// <summary>
/// Class HttpContextExtensions.
/// </summary>
public static class HttpContextExtensions
{
    /// <summary>
    /// Is access token expired as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public static async Task<bool> IsAccessTokenExpiredAsync(this HttpContext context)
    {
        var expiresAt = await context.GetTokenAsync("expires_at");
        if (expiresAt == null) return false;
        var expiresAtAsDateTimeOffset = 
            DateTimeOffset.Parse(expiresAt, CultureInfo.InvariantCulture);
        return (expiresAtAsDateTimeOffset).ToUniversalTime() < DateTime.UtcNow;
    }

    /// <summary>
    /// Get access token as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Logout as an asynchronous operation.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public static async Task LogoutAsync(this HttpContext context)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

    }
}