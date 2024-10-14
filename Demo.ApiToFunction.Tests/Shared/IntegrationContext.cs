using Alba;
using Demo.ApiToFunction.Tests.Fixtures;
using WireMock.Server;

namespace Demo.ApiToFunction.Tests.Shared;

[Collection(Fixtures.ScenariosFixture)]
public abstract class IntegrationContext(WebApiFixture fixture)
{
    public IAlbaHost Host = fixture.AlbaHost;
    public WireMockServer FunctionStub = fixture.FunctionStub;
}

[CollectionDefinition(Fixtures.ScenariosFixture)]
public class ScenarioCollection : ICollectionFixture<WebApiFixture>
{
}

internal static class Fixtures
{
    public const string ScenariosFixture = "Scenarios Test Collection";
}
