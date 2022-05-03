using KC.Models;
namespace KC.WebApplication.Data;
using Newtonsoft.Json;

public class ApiService:IApiService
{
   
    private const string ApiRouteScheduleType = "WeatherForecast";
    private readonly string _baseDataApiUri;

    public ApiService(IConfiguration configuration)
    {
        _baseDataApiUri = configuration["ApiUri"];
    }
    
    public async Task<List<WeatherForecast>> GetWeatherForecastAsync()
    {
       
        var apiUri = new Uri($"{_baseDataApiUri}/{ApiRouteScheduleType}/GetWeatherForecast");
        using var client = new HttpClient();
        var response = await client.GetAsync(apiUri);
        if (!response.IsSuccessStatusCode) return new List<WeatherForecast>();
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<WeatherForecast>>(content);
        return result!;
    }
    public async Task<WeatherForecastResponseModel> GetWeatherForecastAsync1()
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

