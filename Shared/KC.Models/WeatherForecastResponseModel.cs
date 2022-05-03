using System.Net;

namespace KC.Models;

public class WeatherForecastResponseModel
{
    public HttpStatusCode  StatusCode { get; set; }
   
    public string? StatusMessage { get; set; }
    public List<WeatherForecast> Data { get; set; }
    public int Count{ get; set; }
    
}