// ***********************************************************************
// Assembly         : KC.WebApi
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="WeatherForecastController.cs" company="KC.WebApi">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.WebApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace KC.WebApi.Controllers;

/// <summary>
/// Class WeatherForecastController.
/// Implements the <see cref="ControllerBase" />
/// </summary>
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{


    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger<WeatherForecastController> _logger;
    /// <summary>
    /// Gets the weather forecast repository.
    /// </summary>
    /// <value>The weather forecast repository.</value>
    private IWeatherForecastRepository WeatherForecastRepository { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WeatherForecastController" /> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="weatherForecastRepository">The weather forecast repository.</param>
    public WeatherForecastController(ILogger<WeatherForecastController> logger,IWeatherForecastRepository weatherForecastRepository)
    {
        _logger = logger;
        WeatherForecastRepository = weatherForecastRepository;
    }

    /// <summary>
    /// Gets the weather forecast.
    /// </summary>
    /// <returns>IActionResult.</returns>
    [HttpGet("GetWeatherForecast", Name = nameof(GetWeatherForecast))]  
    [Produces("application/json")]
    public  IActionResult GetWeatherForecast()
    {
        var remoteIp = HttpContext.Connection.RemoteIpAddress;
        var route = HttpContext.Request.Path;
        _logger.LogInformation($"Accessing {route} from ip {remoteIp}");
        var result =  WeatherForecastRepository.GetForecast(DateTime.Now);
        return Ok(result);
    }

}