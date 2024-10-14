using Demo.Api.DTOs;
using Demo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(
    ILogger<WeatherForecastController> logger,
    WeatherRepository weatherRepository) : ControllerBase
{
    private readonly WeatherRepository _weatherRepository = weatherRepository;
    private readonly ILogger<WeatherForecastController> _logger = logger;
    private static readonly string[] Summaries = [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogInformation("In GetWeatherForecast");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPut(Name = "PutWeatherForecast")]
    public async Task<IActionResult> Put([FromBody] WeatherForecast forecast)
    {
        var forecastDto = new WeatherForecastDto
        {
            PartitionKey = "WeatherForecast",
            RowKey = Guid.NewGuid().ToString(),
            Date = forecast.Date.ToDateTime(TimeOnly.MinValue),
            TemperatureC = forecast.TemperatureC,
            Summary = forecast.Summary
        };

        var response = await _weatherRepository.AddWeatherForecastAsync(forecastDto);
        if (response.IsError)
        {
            _logger.LogError("Failed to insert forecast: {Error}", response.ReasonPhrase);
            return StatusCode((int)response.Status, response.ReasonPhrase);
        }

        return Ok("Forecast inserted successfully.");
    }
}
