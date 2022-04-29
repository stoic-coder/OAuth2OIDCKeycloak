// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="IOAuthInfo.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Security.Data;
using KC.Security.Model;

namespace KC.Security.Interfaces;

/// <summary>
/// Interface IOAuthInfo
/// </summary>
public interface IOAuthInfo
{
    /// <summary>
    /// Gets the idp service.
    /// </summary>
    /// <value>The idp service.</value>
    IIdpService IdpService { get; }
    /// <summary>
    /// Gets or sets the o authentication user.
    /// </summary>
    /// <value>The o authentication user.</value>
    OAuthUser? OAuthUser { get; set; }
    /// <summary>
    /// Gets or sets the default role.
    /// </summary>
    /// <value>The default role.</value>
    OAuthRole? DefaultRole { get; set; }
    /// <summary>
    /// Gets or sets the access token.
    /// </summary>
    /// <value>The access token.</value>
    string? AccessToken { get; set; }
    /// <summary>
    /// Gets the encrypted access token.
    /// </summary>
    /// <value>The encrypted access token.</value>
    string? EncryptedAccessToken { get;}
    /// <summary>
    /// Gets or sets the o authentication roles.
    /// </summary>
    /// <value>The o authentication roles.</value>
    public List<OAuthRole>? OAuthRoles { get; set; }
    /// <summary>
    /// Gets or sets the o authentication user list.
    /// </summary>
    /// <value>The o authentication user list.</value>
    public List<OAuthUser>? OAuthUserList { get; set; }
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    string? UserId { get; set; }
    /// <summary>
    /// Gets a value indicating whether this instance is default role authorized.
    /// </summary>
    /// <value><c>true</c> if this instance is default role authorized; otherwise, <c>false</c>.</value>
    bool IsDefaultRoleAuthorized { get; }
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets or sets a value indicating whether [show preview].
    /// </summary>
    /// <value><c>true</c> if [show preview]; otherwise, <c>false</c>.</value>
    bool ShowPreview { get; set; }
    /// <summary>
    /// Initializes the asynchronous.
    /// </summary>
    /// <param name="defaultRoleName">Default name of the role.</param>
    /// <param name="accessToken">The access token.</param>
    /// <returns>Task.</returns>
    Task InitializeAsync(string defaultRoleName,string? accessToken = null);
    /// <summary>
    /// Initializes the asynchronous.
    /// </summary>
    /// <param name="getSavedDefaultRoleAsync">The get saved default role asynchronous.</param>
    /// <param name="saveDefaultRoleAsync">The save default role asynchronous.</param>
    /// <returns>Task.</returns>
    Task InitializeAsync(Func<string, Task<string>> getSavedDefaultRoleAsync,Func<string,string, Task<bool>> saveDefaultRoleAsync);
}