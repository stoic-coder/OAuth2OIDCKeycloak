// ***********************************************************************
// Assembly         : KC.WebApplication
// Author           : stoic-coder feat. Nisha Hans
// Created          : 04-29-2022
//
// Last Modified By : stoic-coder feat. Nisha Hans
// Last Modified On : 04-29-2022
// ***********************************************************************
// <copyright file="WeatherForecastViewBase.cs" company="KC.WebApplication">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using KC.Models;
using KC.WebApplication.Data;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace KC.WebApplication.Components;

/// <summary>
/// Class WeatherForecastViewBase.
/// Implements the <see cref="ComponentBase" />
/// </summary>
/// <seealso cref="ComponentBase" />
public class WeatherForecastViewBase : ComponentBase
{

#pragma warning disable CS8618
    /// <summary>
    /// Gets or sets the API service.
    /// </summary>
    /// <value>The API service.</value>
    [Inject] private IApiService ApiService { get; set; }
#pragma warning restore CS8618

#pragma warning disable CS8618
    /// <summary>
    /// Gets or sets the forecasts.
    /// </summary>
    /// <value>The forecasts.</value>
    protected List<WeatherForecast> Forecasts{ get; set; }
#pragma warning restore CS8618

    /// <summary>
    /// On initialized as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        Forecasts = await ApiService.GetWeatherForecastAsync();
    }
    /// <summary>
    /// Loads the data.
    /// </summary>
    /// <param name="args">The arguments.</param>
    protected async Task LoadData(LoadDataArgs args)
    {
        Forecasts = Forecasts = await ApiService.GetWeatherForecastAsync();
       
    }
}