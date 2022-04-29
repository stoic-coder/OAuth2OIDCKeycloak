using System.Reflection.Metadata.Ecma335;

namespace KC.Security.Model;

public class OAuthRole
{
    public OAuthRole(string name, string resourceName)
    {
        Name = name;
        ResourceName = resourceName;
    }

    public OAuthRole(string name)
    {
        Name = name;
        ResourceName = "none";
    }

    public string Name { get; }
    public string ResourceName { get; }    
}