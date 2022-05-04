using System.Net;

namespace KC.Library;

public class WeatherForecastResponseModel
{
    public HttpStatusCode  StatusCode { get; init; }
   
    public string? StatusMessage { get; init; }
    public List<WeatherForecast>? Data { get; init; }
    public int Count{ get; init; }
    
}