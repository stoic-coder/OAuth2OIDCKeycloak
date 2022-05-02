using KC.WebApi.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace KC.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
   

    private readonly ILogger<WeatherForecastController> _logger;
    private IWeatherForecastRepository WeatherForecastRepository { get; }

    public WeatherForecastController(ILogger<WeatherForecastController> logger,IWeatherForecastRepository weatherForecastRepository)
    {
        _logger = logger;
        WeatherForecastRepository = weatherForecastRepository;
    }
    
    [Authorize]
    [HttpGet("GetWeatherForecast", Name = nameof(GetWeatherForecast))]  
    [Produces("application/json")]
    public  IActionResult GetWeatherForecast()
    {
        var remoteIp = HttpContext.Connection.RemoteIpAddress;
        var route = HttpContext.Request.Path;
        _logger.LogInformation($"Accessing {route} from ip {remoteIp}");
        var result =  WeatherForecastRepository.GetForecast(DateTime.Now);
        return Ok(result);
    }

}