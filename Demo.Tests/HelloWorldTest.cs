using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Demo.Tests;

[TestClass]
public class HelloWorldTest
{
    HttpClient client = new HttpClient();
    IContainer container = new ContainerBuilder()
          // Set the image for the container to "testcontainers/helloworld:1.1.0".
          .WithImage("testcontainers/helloworld:1.1.0")
          // Bind port 8080 of the container to a random port on the host.
          .WithPortBinding(8080, true)
          // Wait until the HTTP endpoint of the container is available.
          .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(8080)))
          // Build the container configuration.
          .Build();

    [TestInitialize]
    public async Task TestInit()
    {
        await container.StartAsync();
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        await container.StopAsync();
    }

    [TestMethod]
    public async Task HelloWorldReturnsValidGuid()
    {
        // Arrange
        // Construct the request URI by specifying the scheme, hostname, assigned random host port, and the endpoint "uuid".
        var requestUri = new UriBuilder(Uri.UriSchemeHttp, container.Hostname, container.GetMappedPublicPort(8080), "uuid").Uri;

        // Act
        var guid = await client.GetStringAsync(requestUri);

        // Assert
        Assert.IsTrue(Guid.TryParse(guid, out var _));
    }

    [TestMethod]
    public async Task HelloWorldReturnsValidGuid2()
    {
        // Arrange
        // Construct the request URI by specifying the scheme, hostname, assigned random host port, and the endpoint "uuid".
        var requestUri = new UriBuilder(Uri.UriSchemeHttp, container.Hostname, container.GetMappedPublicPort(8080), "uuid").Uri;

        // Act
        var guid = await client.GetStringAsync(requestUri);

        // Assert
        Assert.IsTrue(Guid.TryParse(guid, out var _));
    }
}
