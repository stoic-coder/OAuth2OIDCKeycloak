using KC.Models;
namespace KC.WebApplication.Data;
using Newtonsoft.Json;

public class WeatherForecastService:IWeatherForecastService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private const string ApiRouteScheduleType = "WeatherForecast";
    private readonly string _baseDataApiUri;

    public WeatherForecastService(IConfiguration configuration,IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _baseDataApiUri = configuration["ApiUri"];
    }
    
    public async Task<List<WeatherForecast>> GetWeatherForecastAsync()
    {
        var apiUri = new Uri($"{_baseDataApiUri}/{ApiRouteScheduleType}/GetWeatherForecast");
        using var client = _httpClientFactory.CreateClient(nameof(WeatherForecastService));
        var response = await client.GetAsync(apiUri);
        if (!response.IsSuccessStatusCode) return new List<WeatherForecast>();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
        return result!;
    }

   
}

