using Azure;
using Azure.Data.Tables;
using Demo.Api.DTOs;
using Demo.Api.Repositories;
using Testcontainers.Azurite;

namespace Demo.Tests;

[TestClass]
public class AzuriteTest
{
    public const string DefaultTableName = "TableName";
    private static readonly AzuriteContainer container = new AzuriteBuilder().Build();
    private static TableClient? tableClient;
    private static WeatherRepository? weatherRepo;

    [ClassInitialize]
    public static async Task ClassInit(TestContext _)
    {
        // Start container for all tests in class
        await container.StartAsync();
        // Initialize TableClient
        var tableServiceClient = new TableServiceClient(container.GetConnectionString());
        tableServiceClient.CreateTableIfNotExists(DefaultTableName);
        tableClient = tableServiceClient.GetTableClient(DefaultTableName);

        weatherRepo = new WeatherRepository(tableClient);
    }

    [ClassCleanup]
    public static async Task ClassCleanup()
    {
        await container.StopAsync();
    }

    [TestMethod]
    public async Task ShouldPutObjectInTable()
    {
        // Arrange
        var forecastDto = new WeatherForecastDto()
        {
            PartitionKey = Guid.NewGuid().ToString(),
            RowKey = Guid.NewGuid().ToString(),
            ETag = ETag.All,
            Date = DateTime.UtcNow,
            TemperatureC = 32,
            Summary = "TestSummary",
        };

        // Act
        var res = await weatherRepo!.AddWeatherForecastAsync(forecastDto);
        var entityResponse = await tableClient!.GetEntityAsync<WeatherForecastDto>(forecastDto.PartitionKey, forecastDto.RowKey);
        var entity = entityResponse.Value;

        // Assert
        Assert.AreEqual(204, res.Status);
        Assert.IsNotNull(entity);
        Assert.AreEqual(forecastDto.RowKey, entity.RowKey);
    }

    [TestMethod]
    public async Task ShouldDeleteObjectFromTable()
    {
        // Arrange
        var forecastDto = new WeatherForecastDto()
        {
            PartitionKey = Guid.NewGuid().ToString(),
            RowKey = Guid.NewGuid().ToString(),
            ETag = ETag.All,
            Date = DateTime.UtcNow,
            TemperatureC = 32,
            Summary = "TestSummary",
        };
        await tableClient!.AddEntityAsync(forecastDto);

        // Act
        var res = await weatherRepo!.DeleteWeatherForecastAsync(forecastDto.PartitionKey, forecastDto.RowKey);

        // Assert
        Assert.AreEqual(204, res.Status);
        await Assert.ThrowsExceptionAsync<RequestFailedException>(
            () => tableClient!.GetEntityAsync<WeatherForecastDto>(forecastDto.PartitionKey, forecastDto.RowKey)
        );
    }
}
