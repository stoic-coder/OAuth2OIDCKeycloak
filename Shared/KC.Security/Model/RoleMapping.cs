// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="RoleMapping.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
// ReSharper disable StringLiteralTypo

namespace KC.Security.Model;

/// <summary>
/// Class RoleMapping.
/// </summary>
public class RoleMapping
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleMapping"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="description">The description.</param>
    /// <param name="composite">if set to <c>true</c> [composite].</param>
    /// <param name="clientRole">if set to <c>true</c> [client role].</param>
    /// <param name="containerId">The container identifier.</param>
    public RoleMapping(string id, string name, string description, bool composite, bool clientRole, string containerId)
    {
        Id = id;
        Name = name;
        Description = description;
        Composite = composite;
        ClientRole = clientRole;
        ContainerId = containerId;
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
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [JsonProperty("description")] 
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="RoleMapping"/> is composite.
    /// </summary>
    /// <value><c>true</c> if composite; otherwise, <c>false</c>.</value>
    [JsonProperty("composite")] 
    public bool Composite { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether [client role].
    /// </summary>
    /// <value><c>true</c> if [client role]; otherwise, <c>false</c>.</value>
    [JsonProperty("clientrole")] 
    public bool ClientRole { get; set; }
    /// <summary>
    /// Gets or sets the container identifier.
    /// </summary>
    /// <value>The container identifier.</value>
    [JsonProperty("containerid")] 
    public string ContainerId { get; set; }
}