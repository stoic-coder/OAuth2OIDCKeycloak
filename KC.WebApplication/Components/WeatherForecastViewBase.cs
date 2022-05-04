using System.Security.Claims;
using KC.Library;
using KC.WebApplication.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KC.WebApplication.Components;

public class WeatherForecastViewBase : ComponentBase
{

#pragma warning disable CS8618
    [Inject] private IWeatherForecastService ApiService { get; set; }

    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

#pragma warning restore CS8618
    protected WeatherForecastResponseModel? WeatherForecastResponse { get; set; }

    private List<Claim>? Claims { get; set; }
    [Inject] private IWeatherForecastService? WeatherForecastService { get; set; }

    public string? PreferredUserName { get; set; } = "Unknown";

    protected List<WeatherForecast>? Forecasts { get; private set; }


    protected override async Task OnInitializedAsync()
    {
       
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            Claims = authState.User.Claims.ToList();
            var preferredUserNameClaim = authState.User.FindFirst("preferred_username");
            if (preferredUserNameClaim is not null)
            {
                PreferredUserName = preferredUserNameClaim.Value;
            }

            var claim = Claims.FirstOrDefault(c => c.Value == "ook-weather");
            if (claim is not null)
            {
                HasOokWeatherClaim = true;
            }

            if (HasOokWeatherClaim)
            {
                WeatherForecastResponse=await ApiService.GetWeatherForecastAsync();
            }
    }

    public bool HasOokWeatherClaim { get; set; }
}