using KC.Models;
using KC.WebApi.Repository.Interfaces;

namespace KC.WebApi.Repository;

public class WeatherForecastRepository:IWeatherForecastRepository
{
    public WeatherForecastRepository(ILogger<WeatherForecastRepository> logger)
    {
        Logger = logger;
        
    }
    private ILogger<WeatherForecastRepository> Logger { get; }
    private static readonly string[] Summaries = new[]
    {
       "Regen",  "Sonnig", "Warm"
    };

      public  List<WeatherForecast> GetForecast(DateTime startDate)
        {
            var resultList = new List<WeatherForecast>();
            for (var i = 1; i <= 30; i++)
            {
                resultList.Add(new WeatherForecast()
                {
                    Id = i,
                    Date = startDate.AddDays(i),
                    TemperatureC = Random.Shared.Next(4, 17),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                });
            }
            return resultList;
        }
}