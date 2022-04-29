// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="IdpService.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using KC.Security.AuthorizationHelpers;
using KC.Security.Extensions;
using KC.Security.Model;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace KC.Security.Data;

/// <summary>
/// Class IdpService.
/// Implements the <see cref="KC.Security.Data.IIdpService" />
/// </summary>
/// <seealso cref="KC.Security.Data.IIdpService" />
public class IdpService : IIdpService
{
    /// <summary>
    /// The HTTP context accessor
    /// </summary>
    private readonly IHttpContextAccessor _httpContextAccessor;
    /// <summary>
    /// The client factory
    /// </summary>
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdpService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    /// <param name="clientFactory">The client factory.</param>
    /// <param name="logger">The logger.</param>
    public IdpService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor,
        IHttpClientFactory clientFactory, ILogger<IdpService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _clientFactory = clientFactory;
        Logger = logger;
        ApiUri = new Uri($"{configuration["IDPBaseUri"]}");
        Realm = $"{configuration["IDPRealm"]}";
        ClientId = configuration.GetValue<string>("Options:ClientId");
        ClientSecret = configuration.GetValue<string>("Options:ClientSecret");
    }

    /// <summary>
    /// Gets or sets the client secret.
    /// </summary>
    /// <value>The client secret.</value>
    private string ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the client identifier.
    /// </summary>
    /// <value>The client identifier.</value>
    private string ClientId { get; set; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    private ILogger<IdpService> Logger { get; }
    /// <summary>
    /// Gets the realm.
    /// </summary>
    /// <value>The realm.</value>
    private string Realm { get; }
    /// <summary>
    /// Gets the API URI.
    /// </summary>
    /// <value>The API URI.</value>
    private Uri ApiUri { get; }

    /// <summary>
    /// Get user list as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
    public async Task<List<OAuthUser>?> GetUserListAsync()
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/users");
        using var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUri);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        var userList = JsonConvert.DeserializeObject<List<OAuthUser>>(content);
        return userList ?? new List<OAuthUser>();
    }

    /// <summary>
    /// Get group member list as an asynchronous operation.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
    public async Task<List<OAuthUser>?> GetGroupMemberListAsync(string? groupId)
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/groups/{groupId}/members");
        using var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUri);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<List<OAuthUser>>(content);
        return user;
    }

    /// <summary>
    /// Get user as an asynchronous operation.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A Task&lt;OAuthUser&gt; representing the asynchronous operation.</returns>
    public async Task<OAuthUser?> GetUserAsync(string? userId)
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/users/{userId}");
        using var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUri);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<OAuthUser>(content);
        return user;
    }

    /// <summary>
    /// Get group list as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;IEnumerable`1&gt; representing the asynchronous operation.</returns>
    public async Task<IEnumerable<OAuthGroup>?> GetGroupListAsync()
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/groups");
        using var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUri);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        var groupList = JsonConvert.DeserializeObject<List<OAuthGroup>>(content);
        return groupList ?? new List<OAuthGroup>();
    }

    /// <summary>
    /// Get group role mapping as an asynchronous operation.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <returns>A Task&lt;OAuthGroupRoleMapping&gt; representing the asynchronous operation.</returns>
    public async Task<OAuthGroupRoleMapping?> GetGroupRoleMappingAsync(string? groupId)
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/groups/{groupId}/role-mappings");
        using var client = new HttpClient();
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUri);
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        var response = await client.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode) return null;
        var content = await response.Content.ReadAsStringAsync();
        var user = JsonConvert.DeserializeObject<OAuthGroupRoleMapping>(content);
        return user;
    }

    /// <summary>
    /// Get token introspect information as an asynchronous operation.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <returns>A Task&lt;Tuple`2&gt; representing the asynchronous operation.</returns>
    public async Task<Tuple<string?, Dictionary<string, object?>?>> GetTokenIntrospectInfoAsync(string? accessToken = null)
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/realms/{Realm}/protocol/openid-connect/token/introspect");
        try
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                if (_httpContextAccessor.HttpContext == null)
                    return new Tuple<string?, Dictionary<string, object?>?>(string.Empty, new Dictionary<string, object?>());

                accessToken = await _httpContextAccessor.HttpContext.GetAccessTokenAsync();
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                using var client = new HttpClient();
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUri);
                var content = new[]
                {
                    new KeyValuePair<string, string>("token", $"{accessToken}"),
                    new KeyValuePair<string, string>("client_id", $"{ClientId}"),
                    new KeyValuePair<string, string>("client_secret", $"{ClientSecret}"),
                };
                requestMessage.Content = new FormUrlEncodedContent(content);
                var response = await client.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    AccessToken = accessToken;
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object?>>(responseContent);
                    return new Tuple<string?, Dictionary<string, object?>?>(accessToken, dictionary);
                }
            }
        }
        catch (Exception e)
        {
#pragma warning disable CA2254
            Logger.LogCritical(e, e.Message);
#pragma warning restore CA2254
        }

        return new Tuple<string?, Dictionary<string, object?>?>(string.Empty, new Dictionary<string, object?>());
    }

    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    /// <value>The access token.</value>
    public string? AccessToken { get; set; }

    /// <summary>
    /// Logout user as an asynchronous operation.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
    public async Task<bool> LogoutUserAsync(string userId)
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/users/{userId}/logout");
        using var client = _clientFactory.CreateClient(HttpClients.IdpApiClient);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUri);
        var response = await client.SendAsync(requestMessage);
        return response.IsSuccessStatusCode;
    }
}