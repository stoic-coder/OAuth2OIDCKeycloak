﻿// ***********************************************************************
// Assembly         : KC.WebApplication
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="Error.cshtml.cs" company="KC.WebApplication">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KC.WebApplication.Pages;

/// <summary>
/// Class ErrorModel.
/// Implements the <see cref="PageModel" />
/// </summary>
/// <seealso cref="PageModel" />
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    /// <summary>
    /// Gets or sets the request identifier.
    /// </summary>
    /// <value>The request identifier.</value>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether [show request identifier].
    /// </summary>
    /// <value><c>true</c> if [show request identifier]; otherwise, <c>false</c>.</value>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<ErrorModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorModel"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Called when [get].
    /// </summary>
    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}