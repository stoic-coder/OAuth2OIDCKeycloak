using System.Security.Claims;
using KC.Models;
using KC.WebApi.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace KC.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
   

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAuthorizationService _authorizationService;
    private IWeatherForecastRepository WeatherForecastRepository { get; }

    public WeatherForecastController(ILogger<WeatherForecastController> logger,IConfiguration configuration,IWeatherForecastRepository weatherForecastRepository, IAuthorizationService authorizationService)
    {
        IsAuthEnabled = configuration.GetValue("IsAuthEnabled",true);
        _logger = logger;
        _authorizationService = authorizationService;
        WeatherForecastRepository = weatherForecastRepository;
    }

    public bool IsAuthEnabled { get; set; }

    //[Authorize(Policy = Policies.NeedsWeatherForecast)]
    [HttpGet("GetWeatherForecast", Name = nameof(GetWeatherForecast))]  
    [Produces("application/json")]
    public async Task<IActionResult> GetWeatherForecast()
    {
        if (IsAuthEnabled)
        {
            var isAuthorized = await CheckPolicy(User, Policies.NeedsWeatherForecast);
            if (!isAuthorized) return Forbid();
        }
        var remoteIp = HttpContext.Connection.RemoteIpAddress;
        var route = HttpContext.Request.Path;
        _logger.LogInformation($"Accessing {route} from ip {remoteIp}");
        var result =  WeatherForecastRepository.GetForecast(DateTime.Now);
        return Ok(result);
    }
    private async Task<bool> CheckPolicy(ClaimsPrincipal user, string policyName)
    {
        var allowed = await _authorizationService.AuthorizeAsync(user, policyName);
        return allowed is {Succeeded: true};
    }
}