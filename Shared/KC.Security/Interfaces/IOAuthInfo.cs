using KC.Security.Data;
using KC.Security.Model;

namespace KC.Security.Interfaces;

public interface IOAuthInfo
{
    IIdpService IdpService { get; }
    OAuthUser? OAuthUser { get; set; }
    OAuthRole? DefaultRole { get; set; }
    string? AccessToken { get; set; }
    string? EncryptedAccessToken { get;}
    public List<OAuthRole>? OAuthRoles { get; set; } 
    public List<OAuthUser>? OAuthUserList { get; set; }
    string? UserId { get; set; }
    bool IsDefaultRoleAuthorized { get; }
    // ReSharper disable once InconsistentNaming
    bool ShowPreview { get; set; }
    Task InitializeAsync(string defaultRoleName,string? accessToken = null);    
    Task InitializeAsync(Func<string, Task<string>> getSavedDefaultRoleAsync,Func<string,string, Task<bool>> saveDefaultRoleAsync);
}