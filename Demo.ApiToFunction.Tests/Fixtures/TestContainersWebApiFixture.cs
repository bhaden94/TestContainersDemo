using Alba;
using Microsoft.Extensions.Configuration;

namespace Demo.ApiToFunction.Tests.Fixtures;

public class TestContainersWebApiFixture(FunctionFixture fixture) : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;

    public async Task InitializeAsync()
    {
        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>()
                {
                    { "FunctionUrl", $"{fixture.Url}" }
                });
            });
        });
    }

    public async Task DisposeAsync()
    {
        await AlbaHost.DisposeAsync();
    }
}
