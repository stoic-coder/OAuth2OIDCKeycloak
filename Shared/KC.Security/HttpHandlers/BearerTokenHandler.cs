// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="BearerTokenHandler.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Globalization;
using IdentityModel.Client;
using KC.Security.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace KC.Security.HttpHandlers;

/// <summary>
/// Class BearerTokenHandler.
/// Implements the <see cref="System.Net.Http.DelegatingHandler" />
/// </summary>
/// <seealso cref="System.Net.Http.DelegatingHandler" />
public class BearerTokenHandler : DelegatingHandler
    {
    /// <summary>
    /// The HTTP context accessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;
    /// <summary>
    /// The HTTP client factory
    /// </summary>
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="BearerTokenHandler"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <exception cref="System.ArgumentNullException">httpContextAccessor</exception>
    /// <exception cref="System.ArgumentNullException">httpClientFactory</exception>
    public BearerTokenHandler(IHttpContextAccessor httpContextAccessor,
                   IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpClientFactory = httpClientFactory ??
                 throw new ArgumentNullException(nameof(httpClientFactory));
        }


    /// <summary>
    /// Send as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
    /// <returns>A Task&lt;HttpResponseMessage&gt; representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, 
            CancellationToken cancellationToken)
        {
            var accessToken = await GetAccessTokenAsync();

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetBearerToken(accessToken);
            }

            var responseMessage = await base.SendAsync(request, cancellationToken);
            return responseMessage;
        }

    /// <summary>
    /// Get access token as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;System.String&gt; representing the asynchronous operation.</returns>
    private async Task<string?> GetAccessTokenAsync()
        {
            if (_httpContextAccessor
                    .HttpContext != null)
            {
                var expired = await _httpContextAccessor.HttpContext.IsAccessTokenExpiredAsync(); 
                if (!expired)
                {
                        var accessToken = await _httpContextAccessor
                            .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
                        return accessToken;
                }
            }

            var idpClient = _httpClientFactory.CreateClient("IDPClient");

            // get the discovery document
            var discoveryResponse = await idpClient.GetDiscoveryDocumentAsync();

            // refresh the tokens
            if (_httpContextAccessor
                    .HttpContext == null) return null;
            var refreshToken = await _httpContextAccessor
                .HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var refreshResponse = await idpClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,
                    ClientId = "imagegalleryclient",
                    ClientSecret = "secret",
                    RefreshToken = refreshToken
                });

            // store the tokens             
            var updatedTokens = new List<AuthenticationToken>
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = refreshResponse.IdentityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = refreshResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = refreshResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = (DateTime.UtcNow + TimeSpan.FromSeconds(refreshResponse.ExpiresIn)).
                        ToString("o", CultureInfo.InvariantCulture)
                }
            };

            // get authenticate result, containing the current principal & 
            // properties
            var currentAuthenticateResult = await _httpContextAccessor
                .HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // store the updated tokens
            if (currentAuthenticateResult.Properties == null) return refreshResponse.AccessToken;
            currentAuthenticateResult.Properties.StoreTokens(updatedTokens);

            // sign in
            if (currentAuthenticateResult.Principal != null)
                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    currentAuthenticateResult.Principal,
                    currentAuthenticateResult.Properties);

            return refreshResponse.AccessToken;

        }
    }