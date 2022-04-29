// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthGroupRoleMapping.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KC.Security.Model;

/// <summary>
/// Class OAuthGroupRoleMapping.
/// </summary>
public class OAuthGroupRoleMapping
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthGroupRoleMapping"/> class.
    /// </summary>
    /// <param name="clientMappings">The client mappings.</param>
    public OAuthGroupRoleMapping(IEnumerable<OAuthClientMappings> clientMappings)
    {
        ClientMappings = clientMappings;
    }

    /// <summary>
    /// Gets or sets the client mappings.
    /// </summary>
    /// <value>The client mappings.</value>
    [JsonProperty("clientMappings")] 
    public IEnumerable<OAuthClientMappings> ClientMappings { get; set; }
}