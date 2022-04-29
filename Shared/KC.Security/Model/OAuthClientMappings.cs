// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthClientMappings.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace KC.Security.Model;


/// <summary>
/// Class OAuthClientMappings.
/// </summary>
public class OAuthClientMappings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthClientMappings"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="clientName">Name of the client.</param>
    /// <param name="mappings">The mappings.</param>
    public OAuthClientMappings(string id, string clientName, IEnumerable<RoleMapping> mappings)
    {
        Id = id;
        ClientName = clientName;
        Mappings = mappings;
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [JsonProperty("id")]
    public string Id { get; set; }
    /// <summary>
    /// Gets or sets the name of the client.
    /// </summary>
    /// <value>The name of the client.</value>
    [JsonProperty("client")]
    public string ClientName { get; set; }
    /// <summary>
    /// Gets or sets the mappings.
    /// </summary>
    /// <value>The mappings.</value>
    [JsonProperty("mappings")]
    public IEnumerable<RoleMapping> Mappings { get; set; }
}