// ***********************************************************************
// Assembly         : KC.WebApplication
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="IApiService.cs" company="KC.WebApplication">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Models;

namespace KC.WebApplication.Data;

/// <summary>
/// Interface IApiService
/// </summary>
public interface IApiService
{
    /// <summary>
    /// Gets the weather forecast asynchronous.
    /// </summary>
    /// <returns>Task&lt;List&lt;WeatherForecast&gt;&gt;.</returns>
    Task<List<WeatherForecast>> GetWeatherForecastAsync();
}