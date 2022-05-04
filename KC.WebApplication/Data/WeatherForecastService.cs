using KC.Library;

namespace KC.WebApplication.Data;
using Newtonsoft.Json;

public class WeatherForecastService:IWeatherForecastService
{
   
    private const string ApiRouteScheduleType = "WeatherForecast";
    private readonly string _baseDataApiUri;

    public WeatherForecastService(IConfiguration configuration)
    {
        _baseDataApiUri = configuration["ApiUri"];
    }
    
   
    public async Task<WeatherForecastResponseModel> GetWeatherForecastAsync()
    {
       
        var apiUri = new Uri($"{_baseDataApiUri}/{ApiRouteScheduleType}/GetWeatherForecast");
        using var client = new HttpClient();
        var response = await client.GetAsync(apiUri);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
        return  new WeatherForecastResponseModel()
        {
            StatusCode = response.StatusCode,
            StatusMessage =response.StatusCode.ToString(),
            Data = result,
            Count = result?.Count() ?? 0
        };
    }
   
}

