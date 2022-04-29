// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="OAuthRole.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Reflection.Metadata.Ecma335;

namespace KC.Security.Model;

/// <summary>
/// Class OAuthRole.
/// </summary>
public class OAuthRole
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRole"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="resourceName">Name of the resource.</param>
    public OAuthRole(string name, string resourceName)
    {
        Name = name;
        ResourceName = resourceName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthRole"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public OAuthRole(string name)
    {
        Name = name;
        ResourceName = "none";
    }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; }
    /// <summary>
    /// Gets the name of the resource.
    /// </summary>
    /// <value>The name of the resource.</value>
    public string ResourceName { get; }    
}