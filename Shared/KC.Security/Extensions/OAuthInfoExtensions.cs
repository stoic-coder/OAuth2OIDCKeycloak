using KC.Security.Interfaces;
using KC.Security.Model;

namespace KC.Security.Extensions;

public static class OAuthInfoExtensions
{
    public static async Task<List<OAuthGroup>> GetGroupsForRealmIdAsync(this IOAuthInfo oAuthInfo)
    {
        var groupList = new List<OAuthGroup>();
        var resultList = await oAuthInfo.IdpService.GetGroupListAsync();
        if (resultList != null)
        {
            groupList.AddRange(resultList);
        }
        return groupList;
    }

    public static async Task<List<OAuthRole>> GetRolesForGroupIdAsync(this IOAuthInfo oAuthInfo, string groupId)
    {
        var roleList = new List<OAuthRole>();
        var groupRoleMapping = await oAuthInfo.IdpService.GetGroupRoleMappingAsync(groupId);
        if (groupRoleMapping == null) return roleList;
        foreach (var clientMappings in groupRoleMapping.ClientMappings)
        {
            roleList.AddRange(clientMappings.Mappings.Select(roleMapping => new OAuthRole(roleMapping.Name, clientMappings.ClientName)));
        }
        return roleList;
    }
    public static async Task<List<OAuthUser>> GetGroupMembersForDefaultGroupAsync(this IOAuthInfo oAuthInfo,string defaultGroupName)
    {
        var memberList = new List<OAuthUser>();
        if (oAuthInfo.UserId == null) return memberList;
        var groups = await oAuthInfo.GetGroupsForRealmIdAsync();
        var defaultGroup = groups.FirstOrDefault(g => g.Name == defaultGroupName);
        if (defaultGroup == null) return memberList;
        var members = await oAuthInfo.IdpService.GetGroupMemberListAsync(defaultGroup.Id);
        if (members != null)
        {
            memberList.AddRange(members);
        }
        return memberList;
    }
}