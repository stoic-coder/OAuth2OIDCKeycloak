using Newtonsoft.Json;
using KC.Security.Models;

namespace KC.Security.Model;

public class OAuthUser
{
    [JsonProperty("id")]
    public string? UserId { get; set; }
    [JsonProperty("createdTimestamp")]
    public long CreatedTimestamp { get; set; }
    [JsonProperty("username")]
    public string? Username { get; set; }
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
    [JsonProperty("totp")]
    public bool Totp { get; set; }
    [JsonProperty("emailVerified")]
    public bool EmailVerified { get; set; }
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }
    [JsonProperty("lastName")]
    public string? LastName { get; set; }
    [JsonProperty("email")]
    public string? Email { get; set; }
    [JsonProperty("notBefore")]
    public int NotBefore { get; set; }
    [JsonProperty("access")]
    public OAuthUserAccess? OAuthUserAccess { get; set; }

    public string FullName =>$"{FirstName + " " + LastName}";
}