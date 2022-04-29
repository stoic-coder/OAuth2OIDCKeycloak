// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthGroup.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace KC.Security.Model;

/// <summary>
/// Class OAuthGroup.
/// </summary>
public class OAuthGroup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthGroup"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="path">The path.</param>
    public OAuthGroup(string id, string name, string path)
    {
        Id = id;
        Name = name;
        Path = path;
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [JsonProperty("id")]
    public string Id { get; set; }
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [JsonProperty("name")]
    public string Name { get; set; }
    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    /// <value>The path.</value>
    [JsonProperty("path")]
    public string Path { get; set; }
}