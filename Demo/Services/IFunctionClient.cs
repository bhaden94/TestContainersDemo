namespace Demo.Api.Services;

public interface IFunctionClient
{
    Task<HttpResponseMessage> GetToFunctionClientAsync();
    Task<HttpResponseMessage> PostToFunctionClientAsync();
}
