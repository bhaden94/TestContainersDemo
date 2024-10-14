using Azure;
using Azure.Data.Tables;
using Demo.Api.DTOs;

namespace Demo.Api.Repositories;

public class WeatherRepository(TableClient tableClient)
{
    private readonly TableClient _tableClient = tableClient;

    public async Task<Response> AddWeatherForecastAsync(WeatherForecastDto forecast)
    {
        return await _tableClient.AddEntityAsync(forecast);
    }

    public async Task<Response> DeleteWeatherForecastAsync(string partitionKey, string rowKey)
    {
        return await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
    }
}
