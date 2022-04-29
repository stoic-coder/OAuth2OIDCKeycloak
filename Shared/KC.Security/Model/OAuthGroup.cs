using Newtonsoft.Json;

namespace KC.Security.Model;

public class OAuthGroup
{
    public OAuthGroup(string id, string name, string path)
    {
        Id = id;
        Name = name;
        Path = path;
    }

    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("path")]
    public string Path { get; set; }
}