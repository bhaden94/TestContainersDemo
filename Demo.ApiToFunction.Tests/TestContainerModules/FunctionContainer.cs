using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Testcontainers.Azurite;

namespace Demo.ApiToFunction.Tests.TestContainerModules;

public class FunctionContainer : IAsyncDisposable
{
    private readonly AzuriteContainer _azuriteContainerInstance = new AzuriteBuilder().Build();
    // image build
    private readonly IFutureDockerImage _azurefuncDockerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), "Demo.Function")
            .WithDockerfile("Dockerfile")
            .WithBuildArgument(
                 "RESOURCE_REAPER_SESSION_ID",
                 ResourceReaper.DefaultSessionId.ToString("D"))
            .Build();
    private IContainer? azureFunctionsContainerInstance;

    public string Hostname => azureFunctionsContainerInstance!.Hostname;
    public ushort MappedPort => azureFunctionsContainerInstance!.GetMappedPublicPort(80);
    public string Url => $"http://{Hostname}:{MappedPort}";

    public async Task InitializeAsync()
    {
        await _azuriteContainerInstance.StartAsync();

        // image build
        await _azurefuncDockerImage.CreateAsync();

        azureFunctionsContainerInstance = new ContainerBuilder()
            // image build
            .WithImage(_azurefuncDockerImage)
            //.WithImage("functionapp:latest")
            .WithEnvironment("AzureWebJobsStorage", _azuriteContainerInstance.GetConnectionString())
            .WithPortBinding(80, true)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r.ForPort(80)))
            .Build();
        await azureFunctionsContainerInstance.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await azureFunctionsContainerInstance!.DisposeAsync();
        // image build
        await _azurefuncDockerImage.DisposeAsync();
        await _azuriteContainerInstance!.DisposeAsync();
    }
}
