// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="IIdpService.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Security.Model;

namespace KC.Security.Data;

/// <summary>
/// Interface IIdpService
/// </summary>
public interface IIdpService
{
    /// <summary>
    /// Gets the user list asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;List&lt;OAuthUser&gt;&gt;&gt;.</returns>
    Task<List<OAuthUser>?> GetUserListAsync();
    /// <summary>
    /// Gets the group member list asynchronous.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <returns>Task&lt;System.Nullable&lt;List&lt;OAuthUser&gt;&gt;&gt;.</returns>
    Task<List<OAuthUser>?> GetGroupMemberListAsync(string? groupId);
    /// <summary>
    /// Gets the user asynchronous.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>Task&lt;System.Nullable&lt;OAuthUser&gt;&gt;.</returns>
    Task<OAuthUser?> GetUserAsync(string? userId);
    /// <summary>
    /// Gets the group list asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;IEnumerable&lt;OAuthGroup&gt;&gt;&gt;.</returns>
    Task<IEnumerable<OAuthGroup>?> GetGroupListAsync();
    /// <summary>
    /// Gets the group role mapping asynchronous.
    /// </summary>
    /// <param name="groupId">The group identifier.</param>
    /// <returns>Task&lt;System.Nullable&lt;OAuthGroupRoleMapping&gt;&gt;.</returns>
    Task<OAuthGroupRoleMapping?> GetGroupRoleMappingAsync(string? groupId);
    /// <summary>
    /// Gets the token introspect information asynchronous.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <returns>Task&lt;Tuple&lt;System.Nullable&lt;System.String&gt;, System.Nullable&lt;Dictionary&lt;System.String, System.Nullable&lt;System.Object&gt;&gt;&gt;&gt;&gt;.</returns>
    Task<Tuple<string?, Dictionary<string, object?>?>> GetTokenIntrospectInfoAsync(string? accessToken = null);
    /// <summary>
    /// Logouts the user asynchronous.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>Task&lt;System.Boolean&gt;.</returns>
    Task<bool> LogoutUserAsync(string userId);
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    /// <value>The access token.</value>
    string? AccessToken { get; set; }
}