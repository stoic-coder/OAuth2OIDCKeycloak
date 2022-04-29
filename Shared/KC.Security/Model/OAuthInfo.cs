#nullable enable
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KC.Security.AuthorizationHelpers;
using KC.Security.Data;
using KC.Security.Helpers;
using KC.Security.Interfaces;

namespace KC.Security.Model;

public class OAuthInfo : IOAuthInfo
{
    private static readonly string[] PossibleDefaultRoles = { Roles.DispatcherRole, Roles.CRMRole };

    public OAuthInfo(IIdpService idpService)
    {
        IdpService = idpService;
    }

    public IIdpService IdpService { get; }
    public OAuthUser? OAuthUser { get; set; }
    public OAuthRole? DefaultRole { get; set; }
    public string? AccessToken { get; set; }
    public bool IsDefaultRoleAuthorized => OAuthRoles!.Contains(DefaultRole!);

    // ReSharper disable once InconsistentNaming
    public bool ShowPreview { get; set; } = false;
    public string? EncryptedAccessToken => AccessToken != null ? StringHelper.Encrypt(AccessToken) : null;

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

    private static string? GetUserIdFromTokenInfo(IReadOnlyDictionary<string, object?>? tokenInfo)
    {
        if (tokenInfo == null) return null;
        if (tokenInfo.TryGetValue("sub", out var userId)) return (string?)userId;
        return null;
    }

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

    public List<OAuthRole>? OAuthRoles { get; set; }
    public List<OAuthUser>? OAuthUserList { get; set; }

    public string? UserId { get; set; }
}