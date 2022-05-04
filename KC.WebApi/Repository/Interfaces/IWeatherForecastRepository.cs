using KC.Models;

namespace KC.WebApi.Repository.Interfaces;

public interface IWeatherForecastRepository
{
    List<WeatherForecast> GetForecast(DateTime startDate);
}