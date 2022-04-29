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

public class IdpService : IIdpService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _clientFactory;

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

    private string ClientSecret { get; set; }

    private string ClientId { get; set; }

    private ILogger<IdpService> Logger { get; }
    private string Realm { get; }
    private Uri ApiUri { get; }

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

    public string? AccessToken { get; set; }

    public async Task<bool> LogoutUserAsync(string userId)
    {
        var apiUri = new Uri($"{ApiUri.AbsoluteUri}/admin/realms/{Realm}/users/{userId}/logout");
        using var client = _clientFactory.CreateClient(HttpClients.IdpApiClient);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUri);
        var response = await client.SendAsync(requestMessage);
        return response.IsSuccessStatusCode;
    }
}