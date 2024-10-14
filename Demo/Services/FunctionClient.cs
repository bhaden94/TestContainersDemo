
namespace Demo.Api.Services;

public class FunctionClient(
    HttpClient httpClient,
    IConfiguration config
) : IFunctionClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _config = config;

    public Task<HttpResponseMessage> GetToFunctionClientAsync()
    {
        return _httpClient.GetAsync($"{_config["FunctionUrl"]}/api/HttpFunction");
    }

    public Task<HttpResponseMessage> PostToFunctionClientAsync()
    {
        return _httpClient.PostAsync($"{_config["FunctionUrl"]}/api/HttpFunction", null);
    }
}
