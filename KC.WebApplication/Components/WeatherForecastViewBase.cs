using KC.Library;
using KC.WebApplication.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KC.WebApplication.Components;

public class WeatherForecastViewBase : ComponentBase
{

#pragma warning disable CS8618
    [Inject] private IWeatherForecastService ApiService { get; set; }
#pragma warning restore CS8618
    protected WeatherForecastResponseModel WeatherForecastResponse { get; set; } = null!;

    private List<Claim>? Claims { get; set; }
    [Inject] private IWeatherForecastService? WeatherForecastService { get; set; }

    public string? PreferredUserName { get; set; } = "Unknown";

    protected List<WeatherForecast>? Forecasts { get; private set; }


    protected override async Task OnInitializedAsync()
    {
        WeatherForecastResponse=await ApiService.GetWeatherForecastAsync();
     
    }

    public bool HasOokWeatherClaim { get; set; }
}