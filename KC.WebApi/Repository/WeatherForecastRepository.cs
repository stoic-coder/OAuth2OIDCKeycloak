// ***********************************************************************
// Assembly         : KC.WebApi
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="WeatherForecastRepository.cs" company="KC.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Models;
using KC.WebApi.Repository.Interfaces;

namespace KC.WebApi.Repository;

/// <summary>
/// Class WeatherForecastRepository.
/// Implements the <see cref="IWeatherForecastRepository" />
/// </summary>
/// <seealso cref="IWeatherForecastRepository" />
public class WeatherForecastRepository:IWeatherForecastRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherForecastRepository"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public WeatherForecastRepository(ILogger<WeatherForecastRepository> logger)
    {
        Logger = logger;
        
    }
    /// <summary>
    /// Gets the logger.
    /// </summary>
    /// <value>The logger.</value>
    private ILogger<WeatherForecastRepository> Logger { get; }
    /// <summary>
    /// The summaries
    /// </summary>
    private static readonly string[] Summaries = new[]
    {
       "Regen",  "Sonnig", "Warm"
    };

    /// <summary>
    /// Gets the forecast.
    /// </summary>
    /// <param name="startDate">The start date.</param>
    /// <returns>List&lt;WeatherForecast&gt;.</returns>
    public List<WeatherForecast> GetForecast(DateTime startDate)
        {
            var resultList = new List<WeatherForecast>();
            for (var i = 1; i <= 30; i++)
            {
                resultList.Add(new WeatherForecast()
                {
                    Id = i,
                    Date = startDate.AddDays(i),
                    TemperatureC = Random.Shared.Next(4, 17),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
            }
            return resultList;
        }
}