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
    public const string DefaultRole = "hlm-user-role";
    /// <summary>
    /// The admin role
    /// </summary>
    public const string AdminRole = "hlm-admin-role";
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// The CRM role
    /// </summary>
    public const string CRMRole = "hlm-crm-role";
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// The CRM assist role
    /// </summary>
    public const string CRMAssistRole = "hlm-crm-assist-role";
    /// <summary>
    /// The dispatcher role
    /// </summary>
    public const string DispatcherRole = "hlm-dispatcher-role";
    /// <summary>
    /// The data reader
    /// </summary>
    public const string DataReader = "hlm-data-reader-role";
    /// <summary>
    /// The data writer
    /// </summary>
    public const string DataWriter = "hlm-data-writer-role";
    /// <summary>
    /// The work council user
    /// </summary>
    public const string WorkCouncilUser = "hlm-work-council-role";
}