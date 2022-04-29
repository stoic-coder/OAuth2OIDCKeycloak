// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthUserAccess.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace KC.Security.Models;

/// <summary>
/// Class OAuthUserAccess.
/// </summary>
public class OAuthUserAccess
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is manage group membership.
    /// </summary>
    /// <value><c>true</c> if this instance is manage group membership; otherwise, <c>false</c>.</value>
    [JsonProperty("manageGroupMembership")]
    public bool IsManageGroupMembership { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is view.
    /// </summary>
    /// <value><c>true</c> if this instance is view; otherwise, <c>false</c>.</value>
    [JsonProperty("view")]
    public bool IsView { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is map roles.
    /// </summary>
    /// <value><c>true</c> if this instance is map roles; otherwise, <c>false</c>.</value>
    [JsonProperty("mapRoles")]
    public bool IsMapRoles { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is impersonate.
    /// </summary>
    /// <value><c>true</c> if this instance is impersonate; otherwise, <c>false</c>.</value>
    [JsonProperty("impersonate")]
    public bool IsImpersonate { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is manage.
    /// </summary>
    /// <value><c>true</c> if this instance is manage; otherwise, <c>false</c>.</value>
    [JsonProperty("manage")]
    public bool IsManage { get; set; }
}