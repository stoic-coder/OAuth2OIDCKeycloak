// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthInfo.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KC.Security.AuthorizationHelpers;
using KC.Security.Data;
using KC.Security.Helpers;
using KC.Security.Interfaces;

namespace KC.Security.Model;

/// <summary>
/// Class OAuthInfo.
/// Implements the <see cref="IOAuthInfo" />
/// </summary>
/// <seealso cref="IOAuthInfo" />
public class OAuthInfo : IOAuthInfo
{
    /// <summary>
    /// The possible default roles
    /// </summary>
    private static readonly string[] PossibleDefaultRoles = { Roles.DefaultRole, Roles.DataReader };

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthInfo"/> class.
    /// </summary>
    /// <param name="idpService">The idp service.</param>
    public OAuthInfo(IIdpService idpService)
    {
        IdpService = idpService;
    }

    /// <summary>
    /// Gets the idp service.
    /// </summary>
    /// <value>The idp service.</value>
    public IIdpService IdpService { get; }
    /// <summary>
    /// Gets or sets the o authentication user.
    /// </summary>
    /// <value>The o authentication user.</value>
    public OAuthUser? OAuthUser { get; set; }
    /// <summary>
    /// Gets or sets the default role.
    /// </summary>
    /// <value>The default role.</value>
    public OAuthRole? DefaultRole { get; set; }
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    /// <value>The access token.</value>
    public string? AccessToken { get; set; }
    /// <summary>
    /// Gets a value indicating whether this instance is default role authorized.
    /// </summary>
    /// <value><c>true</c> if this instance is default role authorized; otherwise, <c>false</c>.</value>
    public bool IsDefaultRoleAuthorized => OAuthRoles!.Contains(DefaultRole!);

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets or sets a value indicating whether [show preview].
    /// </summary>
    /// <value><c>true</c> if [show preview]; otherwise, <c>false</c>.</value>
    public bool ShowPreview { get; set; } = false;
    /// <summary>
    /// Gets the encrypted access token.
    /// </summary>
    /// <value>The encrypted access token.</value>
    public string? EncryptedAccessToken => AccessToken != null ? StringHelper.Encrypt(AccessToken) : null;

    /// <summary>
    /// Initialize as an asynchronous operation.
    /// </summary>
    /// <param name="defaultRoleName">Default name of the role.</param>
    /// <param name="accessToken">The access token.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(string defaultRoleName,string? accessToken = null)
    {
        var tokenInfo = await IdpService.GetTokenIntrospectInfoAsync(accessToken);
        AccessToken = accessToken ?? IdpService.AccessToken;
        OAuthRoles = new List<OAuthRole>();
        OAuthRoles.AddRange(GetRolesFromTokenInfo(tokenInfo.Item2));
        OAuthUserList = new List<OAuthUser>();
        var userList = await IdpService.GetUserListAsync();
        if (userList != null) OAuthUserList.AddRange(userList);
        UserId = GetUserIdFromTokenInfo(tokenInfo.Item2);
        if (UserId != null)
        {
            OAuthUser = await IdpService.GetUserAsync(UserId);
            DefaultRole = new OAuthRole(defaultRoleName);
            if (!PossibleDefaultRoles.Contains(DefaultRole!.Name)) DefaultRole = null;
            if (string.IsNullOrEmpty(DefaultRole?.Name))
                foreach (var defaultRole in PossibleDefaultRoles)
                {
                    if (OAuthRoles != null)
                    {
                        var r = OAuthRoles.FirstOrDefault(r => r.Name.Equals(defaultRole));
                        if (r == null) continue;
                        DefaultRole = new OAuthRole(r.Name);
                    }

                    break;
                }
        }
    }

    /// <summary>
    /// Initialize as an asynchronous operation.
    /// </summary>
    /// <param name="getSavedDefaultRoleAsync">The get saved default role asynchronous.</param>
    /// <param name="saveDefaultRoleAsync">The save default role asynchronous.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InitializeAsync(Func<string, Task<string>> getSavedDefaultRoleAsync,
        Func<string, string, Task<bool>> saveDefaultRoleAsync)
    {
        var tokenInfo = await IdpService.GetTokenIntrospectInfoAsync();
        AccessToken = IdpService.AccessToken;
        OAuthRoles = new List<OAuthRole>();
        OAuthRoles.AddRange(GetRolesFromTokenInfo(tokenInfo.Item2));
        OAuthUserList = new List<OAuthUser>();
        var userList = await IdpService.GetUserListAsync();
        if (userList != null) OAuthUserList.AddRange(userList);
        UserId = GetUserIdFromTokenInfo(tokenInfo.Item2);
        if (UserId != null)
        {
            OAuthUser = await IdpService.GetUserAsync(UserId);
            var defaultRoleName = await getSavedDefaultRoleAsync(UserId);
            DefaultRole = new OAuthRole(defaultRoleName);
            if (!PossibleDefaultRoles.Contains(DefaultRole!.Name)) DefaultRole = null;
            if (string.IsNullOrEmpty(DefaultRole?.Name))
                foreach (var defaultRole in PossibleDefaultRoles)
                {
                    if (OAuthRoles != null)
                    {
                        var r = OAuthRoles.FirstOrDefault(r => r.Name.Equals(defaultRole));
                        if (r == null) continue;
                        DefaultRole = new OAuthRole(r.Name);
                    }

                    break;
                }

            if (DefaultRole != null) await saveDefaultRoleAsync(UserId, DefaultRole.Name);
        }
    }

    /// <summary>
    /// Gets the user identifier from token information.
    /// </summary>
    /// <param name="tokenInfo">The token information.</param>
    /// <returns>System.Nullable&lt;System.String&gt;.</returns>
    private static string? GetUserIdFromTokenInfo(IReadOnlyDictionary<string, object?>? tokenInfo)
    {
        if (tokenInfo == null) return null;
        if (tokenInfo.TryGetValue("sub", out var userId)) return (string?)userId;
        return null;
    }

    /// <summary>
    /// Gets the roles from token information.
    /// </summary>
    /// <param name="tokenInfo">The token information.</param>
    /// <returns>IEnumerable&lt;OAuthRole&gt;.</returns>
    private IEnumerable<OAuthRole> GetRolesFromTokenInfo(Dictionary<string, object?>? tokenInfo)
    {
        var resultList = new List<OAuthRole>();
        if (tokenInfo == null) return resultList;
        if (tokenInfo.TryGetValue("active", out var isActive))
            if (isActive != null)
                if (!(bool)isActive)
                    return resultList;

        if (tokenInfo.TryGetValue("realm_access", out var realmAccessRolesData))
        {
            var realmAccessRolesObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(realmAccessRolesData?.ToString()!);
            resultList.AddRange(GetOAuthRoles(realmAccessRolesObject, "realm_access"));
        }

        if (!tokenInfo.TryGetValue("resource_access", out var resourceAccess)) return resultList;
        if (string.IsNullOrEmpty(resourceAccess?.ToString())) return resultList;
        var resourceAccessData = JsonConvert.DeserializeObject<Dictionary<string, object>>(resourceAccess.ToString()!);
        if (resourceAccessData == null) return resultList;
        foreach (var item in resourceAccessData)
        {
            if (!resourceAccessData.TryGetValue(item.Key, out var realmRolesData)) continue;
            var realmManagementRolesObject =
                JsonConvert.DeserializeObject<Dictionary<string, object>>(realmRolesData.ToString()!);
            resultList.AddRange(GetOAuthRoles(realmManagementRolesObject, item.Key));
        }

        return resultList;
    }

    /// <summary>
    /// Gets the o authentication roles.
    /// </summary>
    /// <param name="resourceAccessObject">The resource access object.</param>
    /// <param name="resourceName">Name of the resource.</param>
    /// <returns>IEnumerable&lt;OAuthRole&gt;.</returns>
    private IEnumerable<OAuthRole> GetOAuthRoles(Dictionary<string, object>? resourceAccessObject, string resourceName)
    {
        var resultList = new List<OAuthRole>();
        if (resourceAccessObject == null) return resultList;
        foreach (var item in resourceAccessObject)
        {
            if (item.Value is not JArray jArray) continue;
            var roleList = new List<string>();
            var roles = JsonConvert.DeserializeObject<string[]>(jArray.ToString());
            if (roles != null) roleList.AddRange(roles);
            resultList.AddRange(roleList.Select(roleItem => new OAuthRole(roleItem, resourceName)));
        }

        return resultList;
    }

    /// <summary>
    /// Gets or sets the o authentication roles.
    /// </summary>
    /// <value>The o authentication roles.</value>
    public List<OAuthRole>? OAuthRoles { get; set; }
    /// <summary>
    /// Gets or sets the o authentication user list.
    /// </summary>
    /// <value>The o authentication user list.</value>
    public List<OAuthUser>? OAuthUserList { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    public string? UserId { get; set; }
}