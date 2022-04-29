// ***********************************************************************
// Assembly         : KC.WebApi
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="IWeatherForecastRepository.cs" company="KC.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Models;

namespace KC.WebApi.Repository.Interfaces;

/// <summary>
/// Interface IWeatherForecastRepository
/// </summary>
public interface IWeatherForecastRepository
{
    /// <summary>
    /// Gets the forecast.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <returns>List&lt;WeatherForecast&gt;.</returns>
    List<WeatherForecast> GetForecast(DateTime startDate);
}