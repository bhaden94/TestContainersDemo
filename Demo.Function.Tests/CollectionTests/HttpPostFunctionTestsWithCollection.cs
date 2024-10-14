using Demo.Function.Tests.Collections;
using Demo.Function.Tests.Fixtures;

namespace Demo.Function.Tests.CollectionTests;

[Collection(nameof(FunctionCollection))]
public class HttpPostFunctionTestsWithCollection(FunctionFixture fixture)
{
    [Fact]
    public async Task FunctionRequestReturnsResponseWithSuccessStatusCode()
    {
        HttpClient httpClient = new HttpClient();
        var requestUri = new UriBuilder(
            Uri.UriSchemeHttp,
            fixture.Hostname,
            fixture.MappedPort,
            "api/HttpFunction"
        ).Uri;

        HttpResponseMessage response = await httpClient.PostAsync(requestUri, null);

        Assert.True(response.IsSuccessStatusCode);
    }
}
