using Microsoft.AspNetCore.Authorization;

namespace KC.Library;

public static class Policies
{
    public const string NeedsWeatherForecast = "ook-weather";
    public static AuthorizationPolicy NeedsWeatherForecastPolicy()
        => new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("ook-weather", "ook-weather")
            .Build();
}