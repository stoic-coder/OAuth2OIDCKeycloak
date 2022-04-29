// ***********************************************************************
// Assembly         : KC.Security
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="Roles.cs" company="KC.Security">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace KC.Security.AuthorizationHelpers;

/// <summary>
/// Class Roles.
/// </summary>
public class Roles
{
    /// <summary>
    /// The default role
    /// </summary>
    public const string DefaultRole = "default-role";
    /// <summary>
    /// The admin role
    /// </summary>
    public const string AdminRole = "admin-role";
    /// <summary>
    /// The data reader
    /// </summary>
    public const string DataReader = "data-reader-role";
    /// <summary>
    /// The data writer
    /// </summary>
    public const string DataWriter = "data-writer-role";
}