
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Testcontainers.Azurite;

namespace Demo.Function.Tests;

public class HttpFunctionTest : IAsyncLifetime
{
    private readonly IFutureDockerImage _azureFunctionsDockerImage;
    private IContainer? azureFunctionsContainerInstance;
    private AzuriteContainer? azuriteContainerInstance;

    public HttpFunctionTest()
    {
        _azureFunctionsDockerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), "Demo.Function")
            .WithDockerfile("Dockerfile")
            .WithBuildArgument(
                 "RESOURCE_REAPER_SESSION_ID",
                 ResourceReaper.DefaultSessionId.ToString("D"))
            .Build();
    }

    public async Task InitializeAsync()
    {
        azuriteContainerInstance = new AzuriteBuilder().Build();
        await azuriteContainerInstance.StartAsync();

        await _azureFunctionsDockerImage.CreateAsync();

        azureFunctionsContainerInstance = new ContainerBuilder()
            .WithImage(_azureFunctionsDockerImage)
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
        await _azureFunctionsDockerImage.DisposeAsync();
        await azuriteContainerInstance!.DisposeAsync();
    }

    [Fact]
    public async Task FunctionGetRequestReturnsResponseWithSuccessStatusCode()
    {
        HttpClient httpClient = new HttpClient();
        var requestUri = new UriBuilder(
            Uri.UriSchemeHttp,
            azureFunctionsContainerInstance!.Hostname,
            azureFunctionsContainerInstance.GetMappedPublicPort(80),
            "api/HttpFunction"
        ).Uri;

        HttpResponseMessage response = await httpClient.GetAsync(requestUri);

        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task FunctionPostRequestReturnsResponseWithSuccessStatusCode()
    {
        HttpClient httpClient = new HttpClient();
        var requestUri = new UriBuilder(
            Uri.UriSchemeHttp,
            azureFunctionsContainerInstance!.Hostname,
            azureFunctionsContainerInstance.GetMappedPublicPort(80),
            "api/HttpFunction"
        ).Uri;

        HttpResponseMessage response = await httpClient.PostAsync(requestUri, null);

        Assert.True(response.IsSuccessStatusCode);
    }
}