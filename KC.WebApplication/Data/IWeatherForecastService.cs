using KC.Models;

namespace KC.WebApplication.Data;

public interface IWeatherForecastService
{
    Task<List<WeatherForecast>> GetWeatherForecastAsync();
}