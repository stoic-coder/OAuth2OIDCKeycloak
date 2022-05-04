using KC.Library;

namespace KC.WebApplication.Data;

public interface IWeatherForecastService
{
    Task<List<WeatherForecast>?>? GetWeatherForecastAsync();
}