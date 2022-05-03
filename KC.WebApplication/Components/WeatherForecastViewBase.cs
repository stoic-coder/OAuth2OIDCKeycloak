using KC.Models;
using KC.WebApplication.Data;
using Microsoft.AspNetCore.Components;

namespace KC.WebApplication.Components;

public class WeatherForecastViewBase : ComponentBase
{

#pragma warning disable CS8618
    [Inject] private IWeatherForecastService ApiService { get; set; }
#pragma warning restore CS8618
    protected WeatherForecastResponseModel WeatherForecastResponse { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        WeatherForecastResponse=await ApiService.GetWeatherForecastAsync();
     
    }
}