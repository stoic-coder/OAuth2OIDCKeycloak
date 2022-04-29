using System.Collections.Generic;
using Newtonsoft.Json;

namespace KC.Security.Model;

public class OAuthGroupRoleMapping
{
    public OAuthGroupRoleMapping(IEnumerable<OAuthClientMappings> clientMappings)
    {
        ClientMappings = clientMappings;
    }

    [JsonProperty("clientMappings")] 
    public IEnumerable<OAuthClientMappings> ClientMappings { get; set; }
}