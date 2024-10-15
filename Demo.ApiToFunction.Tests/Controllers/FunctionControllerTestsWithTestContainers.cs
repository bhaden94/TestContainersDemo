using Alba;
using Demo.ApiToFunction.Tests.Fixtures;
using Demo.ApiToFunction.Tests.Shared;

namespace Demo.ApiToFunction.Tests.Controllers;

public class FunctionControllerTestsWithTestContainers(TestContainersWebApiFixture fixture)
    : TestContainerIntegrationContext(fixture)
{
    [Fact]
    public async Task ShouldCallGetToFunctionSuccessfully()
    {
        var result = await this.Host.Scenario(_ =>
        {
            _.Get.Url("/Function");
            _.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task ShouldCallPostToFunctionSuccessfully()
    {
        var result = await this.Host.Scenario(_ =>
        {
            _.Post.Url("/Function");
            _.StatusCodeShouldBeOk();
        });
    }
}
