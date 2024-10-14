using Alba;
using Demo.ApiToFunction.Tests.Collections;
using Demo.ApiToFunction.Tests.Fixtures;

namespace Demo.ApiToFunction.Tests.Shared;


[Collection(nameof(FunctionCollection))]
public abstract class TestContainerIntegrationContext(TestContainersWebApiFixture fixture) : IClassFixture<TestContainersWebApiFixture>
{
    public IAlbaHost Host = fixture.AlbaHost;
}
