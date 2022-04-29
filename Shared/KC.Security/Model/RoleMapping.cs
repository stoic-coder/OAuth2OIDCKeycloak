using Newtonsoft.Json;

namespace KC.Security.Model;

public class RoleMapping
{
    public RoleMapping(string id, string name, string description, bool composite, bool clientRole, string containerId)
    {
        Id = id;
        Name = name;
        Description = description;
        Composite = composite;
        ClientRole = clientRole;
        ContainerId = containerId;
    }

    [JsonProperty("id")] 
    public string Id { get; set; }

    [JsonProperty("name")] 
    public string Name { get; set; }
    [JsonProperty("description")] 
    public string Description { get; set; }
    [JsonProperty("composite")] 
    public bool Composite { get; set; }
    [JsonProperty("clientrole")] 
    public bool ClientRole { get; set; }
    [JsonProperty("containerid")] 
    public string ContainerId { get; set; }
}