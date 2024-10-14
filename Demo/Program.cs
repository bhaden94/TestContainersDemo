using Azure.Data.Tables;
using Demo.Api.Repositories;
using Demo.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(s =>
{
    var connectionString = Environment.GetEnvironmentVariable("AZURITE_CONNECTION_STRING");
    var tableName = Environment.GetEnvironmentVariable("WEATHER_FORECAST_TABLE_NAME");
    return new TableClient(connectionString, tableName);
});
builder.Services.AddTransient<WeatherRepository>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddTransient<IFunctionClient, FunctionClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
