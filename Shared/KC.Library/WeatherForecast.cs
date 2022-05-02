namespace KC.Models;

public class WeatherForecast
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public int TemperatureC { get; init; }

    public string? Summary { get; set; }
}