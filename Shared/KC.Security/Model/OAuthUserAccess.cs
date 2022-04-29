using Newtonsoft.Json;

namespace KC.Security.Models;

public class OAuthUserAccess
{
    [JsonProperty("manageGroupMembership")]
    public bool IsManageGroupMembership { get; set; }
    [JsonProperty("view")]
    public bool IsView { get; set; }
    [JsonProperty("mapRoles")]
    public bool IsMapRoles { get; set; }
    [JsonProperty("impersonate")]
    public bool IsImpersonate { get; set; }
    [JsonProperty("manage")]
    public bool IsManage { get; set; }
}