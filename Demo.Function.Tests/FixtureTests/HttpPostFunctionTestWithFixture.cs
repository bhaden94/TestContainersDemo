using Demo.Function.Tests.Fixtures;

namespace Demo.Function.Tests.FixtureTests;

public class HttpPostFunctionTestWithFixture(FunctionFixture fixture) : IClassFixture<FunctionFixture>
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
