using KC.Security.Model;

namespace KC.Security.Data;

public interface IIdpService
{
    Task<List<OAuthUser>?> GetUserListAsync();
    Task<List<OAuthUser>?> GetGroupMemberListAsync(string? groupId);
    Task<OAuthUser?> GetUserAsync(string? userId);
    Task<IEnumerable<OAuthGroup>?> GetGroupListAsync();
    Task<OAuthGroupRoleMapping?> GetGroupRoleMappingAsync(string? groupId);
    Task<Tuple<string?, Dictionary<string, object?>?>> GetTokenIntrospectInfoAsync(string? accessToken = null);
    Task<bool> LogoutUserAsync(string userId);
    string? AccessToken { get; set; }
}