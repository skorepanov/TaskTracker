using Xunit;

namespace TaskTracker.IntegrationTests;

public abstract class IntegrationTest : IClassFixture<ApiWebApplicationFactory>
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly HttpClient Client;

    public IntegrationTest(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
    }
}