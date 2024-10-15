using Alba;
using Demo.ApiToFunction.Tests.Fixtures;
using Demo.ApiToFunction.Tests.TestContainerModules;

namespace Demo.ApiToFunction.Tests.Shared;

[Collection(TextContainerFixtures.FunctionContainer)]
public abstract class TestContainerIntegrationContext(TestContainersWebApiFixture fixture)
{
    public IAlbaHost Host = fixture.AlbaHost;
    public FunctionContainer FunctionContainer = fixture.FunctionContainer;
}

[CollectionDefinition(TextContainerFixtures.FunctionContainer)]
public class TestContainerCollection : ICollectionFixture<TestContainersWebApiFixture>;

internal static class TextContainerFixtures
{
    public const string FunctionContainer = "Function Test Collection";
}
