// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthUser.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using KC.Security.Models;

namespace KC.Security.Model;

/// <summary>
/// Class OAuthUser.
/// </summary>
public class OAuthUser
{
    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    [JsonProperty("id")]
    public string? UserId { get; set; }
    /// <summary>
    /// Gets or sets the created timestamp.
    /// </summary>
    /// <value>The created timestamp.</value>
    [JsonProperty("createdTimestamp")]
    public long CreatedTimestamp { get; set; }
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [JsonProperty("username")]
    public string? Username { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="OAuthUser"/> is enabled.
    /// </summary>
    /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
    [JsonProperty("enabled")]
    public bool Enabled { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="OAuthUser"/> is totp.
    /// </summary>
    /// <value><c>true</c> if totp; otherwise, <c>false</c>.</value>
    [JsonProperty("totp")]
    public bool Totp { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [email verified].
    /// </summary>
    /// <value><c>true</c> if [email verified]; otherwise, <c>false</c>.</value>
    [JsonProperty("emailVerified")]
    public bool EmailVerified { get; set; }
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    /// <value>The first name.</value>
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }
    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    /// <value>The last name.</value>
    [JsonProperty("lastName")]
    public string? LastName { get; set; }
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    [JsonProperty("email")]
    public string? Email { get; set; }
    /// <summary>
    /// Gets or sets the not before.
    /// </summary>
    /// <value>The not before.</value>
    [JsonProperty("notBefore")]
    public int NotBefore { get; set; }
    /// <summary>
    /// Gets or sets the o authentication user access.
    /// </summary>
    /// <value>The o authentication user access.</value>
    [JsonProperty("access")]
    public OAuthUserAccess? OAuthUserAccess { get; set; }

    /// <summary>
    /// Gets the full name.
    /// </summary>
    /// <value>The full name.</value>
    public string FullName =>$"{FirstName + " " + LastName}";
}