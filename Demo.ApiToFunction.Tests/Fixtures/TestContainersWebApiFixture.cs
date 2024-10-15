using Alba;
using Demo.ApiToFunction.Tests.TestContainerModules;
using Microsoft.Extensions.Configuration;

namespace Demo.ApiToFunction.Tests.Fixtures;

public class TestContainersWebApiFixture : IAsyncLifetime
{
    public IAlbaHost AlbaHost = null!;
    public FunctionContainer FunctionContainer = new();

    public async Task InitializeAsync()
    {
        await FunctionContainer.InitializeAsync();

        AlbaHost = await Alba.AlbaHost.For<Program>(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>()
                {
                    { "FunctionUrl", $"{FunctionContainer.Url}" }
                });
            });
        });
    }

    public async Task DisposeAsync()
    {
        await FunctionContainer.DisposeAsync();
        await AlbaHost.DisposeAsync();
    }
}
