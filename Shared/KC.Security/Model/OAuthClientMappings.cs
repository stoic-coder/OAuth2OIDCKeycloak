using Newtonsoft.Json;

namespace KC.Security.Model;


public class OAuthClientMappings
{
    public OAuthClientMappings(string id, string clientName, IEnumerable<RoleMapping> mappings)
    {
        Id = id;
        ClientName = clientName;
        Mappings = mappings;
    }

    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("client")]
    public string ClientName { get; set; }
    [JsonProperty("mappings")]
    public IEnumerable<RoleMapping> Mappings { get; set; }
}