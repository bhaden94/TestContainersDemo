using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Testcontainers.Azurite;

namespace Demo.Function.Tests.Fixtures;

public class FunctionFixture : IAsyncLifetime
{
    private readonly IFutureDockerImage _azurefuncDockerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), "Demo.Function")
            .WithDockerfile("Dockerfile")
            .WithBuildArgument(
                 "RESOURCE_REAPER_SESSION_ID",
                 ResourceReaper.DefaultSessionId.ToString("D"))
            .Build();
    private IContainer? azureFunctionsContainerInstance;
    private AzuriteContainer? azuriteContainerInstance;

    public string Hostname => azureFunctionsContainerInstance!.Hostname;
    public ushort MappedPort => azureFunctionsContainerInstance!.GetMappedPublicPort(80);

    public async Task InitializeAsync()
    {
        azuriteContainerInstance = new AzuriteBuilder().Build();
        await azuriteContainerInstance.StartAsync();

        await _azurefuncDockerImage.CreateAsync();

        azureFunctionsContainerInstance = new ContainerBuilder()
            .WithImage(_azurefuncDockerImage)
            .WithEnvironment("AzureWebJobsStorage", azuriteContainerInstance.GetConnectionString())
            .WithPortBinding(80, true)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r.ForPort(80)))
            .Build();
        await azureFunctionsContainerInstance.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await azureFunctionsContainerInstance!.DisposeAsync();
        await _azurefuncDockerImage.DisposeAsync();
        await azuriteContainerInstance!.DisposeAsync();
    }
}


