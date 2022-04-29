using KC.Models;

namespace KC.WebApplication.Data;

public interface IApiService
{
    Task<List<WeatherForecast>> GetWeatherForecastAsync();
}