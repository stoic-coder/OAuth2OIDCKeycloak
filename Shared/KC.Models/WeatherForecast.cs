// ***********************************************************************
// Assembly         : KC.Models
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="WeatherForecast.cs" company="KC.Models">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace KC.Models;

/// <summary>
/// Class WeatherForecast.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the date.
    /// </summary>
    /// <value>The date.</value>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the temperature c.
    /// </summary>
    /// <value>The temperature c.</value>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Gets the temperature f.
    /// </summary>
    /// <value>The temperature f.</value>
    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

    /// <summary>
    /// Gets or sets the summary.
    /// </summary>
    /// <value>The summary.</value>
    public string? Summary { get; set; }
}