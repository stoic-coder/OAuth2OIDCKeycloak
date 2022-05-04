using System.Security.Claims;
using KC.Library;
using KC.WebApplication.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace KC.WebApplication.Components;

public class WeatherForecastViewBase : ComponentBase
{
    [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

    private List<Claim>? Claims { get; set; }
    [Inject] private IWeatherForecastService? WeatherForecastService { get; set; }

    public string? PreferredUserName { get; set; } = "Unknown";

    protected List<WeatherForecast>? Forecasts { get; private set; }


    protected override async Task OnInitializedAsync()
    {
        var authState =
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
        Claims = authState.User.Claims.ToList();
        var preferredUserNameCalim = authState.User.FindFirst("preferred_username");
        if (preferredUserNameCalim is not null)
        {
            PreferredUserName = preferredUserNameCalim.Value;
        }

        var claim = Claims.FirstOrDefault(c => c.Value == "ook-weather");
        if (claim is not null)
        {
            HasOokWeatherClaim = true;
        }

        if (HasOokWeatherClaim)
        {
            Forecasts = await WeatherForecastService!.GetWeatherForecastAsync()!;
        }
    }

    public bool HasOokWeatherClaim { get; set; }
}