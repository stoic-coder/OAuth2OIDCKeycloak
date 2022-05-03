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
    protected List<WeatherForecast>? Forecasts{ get; private set; }
#pragma warning restore CS8618
    public WeatherForecastResponseModel WeatherForecastResponse { get; set; }

    protected override async Task OnInitializedAsync()
    {
        WeatherForecastResponse=await ApiService.GetWeatherForecastAsync1();
        Forecasts = await ApiService.GetWeatherForecastAsync();
    }
}