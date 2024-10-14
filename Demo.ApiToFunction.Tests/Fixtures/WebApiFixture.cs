using Alba;
using Demo.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using WireMock.Server;

namespace Demo.ApiToFunction.Tests.Fixtures;

public class WebApiFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;
    public WireMockServer FunctionStub = null!;

    public async Task InitializeAsync()
    {
        FunctionStub = WireMockServer.Start(1234);
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            // Configure all the things
            builder.ConfigureServices((context, services) =>
            {
                // Line below is used for mocking with Moq
                //services.AddTransient(x => CreateMockFunctionClient());
            });
            builder.ConfigureAppConfiguration((context, config) =>
            {
                //config.AddInMemoryCollection(new Dictionary<string, string?>()
                //{
                //    { "FunctionUrl", "http://localhost:1234" }
                //});
            });
        });
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }

    private static IFunctionClient CreateMockFunctionClient()
    {
        var mock = new Mock<IFunctionClient>();

        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"message\": \"Success\"}", System.Text.Encoding.UTF8, "application/json")
        };

        mock.Setup(x => x.GetToFunctionClientAsync()).ReturnsAsync(mockResponse);
        mock.Setup(x => x.PostToFunctionClientAsync()).ReturnsAsync(mockResponse);

        return mock.Object;
    }
}
