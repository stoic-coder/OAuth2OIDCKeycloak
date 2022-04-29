// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthInfoExtensions.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Security.Interfaces;
using KC.Security.Model;

namespace KC.Security.Extensions;

/// <summary>
/// Class OAuthInfoExtensions.
/// </summary>
public static class OAuthInfoExtensions
{
    /// <summary>
    /// Get groups for realm identifier as an asynchronous operation.
    /// </summary>
    /// <param name="oAuthInfo">The o authentication information.</param>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Get roles for group identifier as an asynchronous operation.
    /// </summary>
    /// <param name="oAuthInfo">The o authentication information.</param>
    /// <param name="groupId">The group identifier.</param>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
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
    /// <summary>
    /// Get group members for default group as an asynchronous operation.
    /// </summary>
    /// <param name="oAuthInfo">The o authentication information.</param>
    /// <param name="defaultGroupName">Default name of the group.</param>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
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