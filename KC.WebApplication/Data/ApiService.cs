// ***********************************************************************
// Assembly         : KC.WebApplication
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="ApiService.cs" company="KC.WebApplication">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Models;
namespace KC.WebApplication.Data;
using Newtonsoft.Json;

/// <summary>
/// Class ApiService.
/// Implements the <see cref="KC.WebApplication.Data.IApiService" />
/// </summary>
/// <seealso cref="KC.WebApplication.Data.IApiService" />
public class ApiService:IApiService
{

    /// <summary>
    /// The API route schedule type
    /// </summary>
    private const string ApiRouteScheduleType = "WeatherForecast";
    /// <summary>
    /// The base data API URI
    /// </summary>
    private readonly string _baseDataApiUri;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public ApiService(IConfiguration configuration)
    {
        _baseDataApiUri = configuration["ApiUri"];
    }

    /// <summary>
    /// Get weather forecast as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;List`1&gt; representing the asynchronous operation.</returns>
    public async Task<List<WeatherForecast>> GetWeatherForecastAsync()
    {
        var apiUri = new Uri($"{_baseDataApiUri}/{ApiRouteScheduleType}/GetWeatherForecast");
        using var client = new HttpClient();
        var response = await client.GetAsync(apiUri);
        if (!response.IsSuccessStatusCode) return new List<WeatherForecast>();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
        return result!;
    }

   
}

