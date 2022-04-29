using KC.Models;
using KC.WebApplication.Data;
using Microsoft.AspNetCore.Components;

namespace KC.WebApplication.Components;

public class WeatherForecastViewBase : ComponentBase
{
   
#pragma warning disable CS8618
    [Inject] private IApiService ApiService { get; set; }
#pragma warning restore CS8618
    
#pragma warning disable CS8618
    protected List<WeatherForecast> Forecasts{ get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        Forecasts = await ApiService.GetWeatherForecastAsync();
    }
}