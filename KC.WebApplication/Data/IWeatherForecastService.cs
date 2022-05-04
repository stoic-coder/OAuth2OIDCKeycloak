using KC.Library;

namespace KC.WebApplication.Data;

public interface IWeatherForecastService
{
   
    Task<WeatherForecastResponseModel> GetWeatherForecastAsync();
}