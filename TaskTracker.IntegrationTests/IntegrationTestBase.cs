using Moq;
using TaskTracker.Bll;
using Xunit;

namespace TaskTracker.IntegrationTests;

public abstract class IntegrationTestBase
    : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected readonly Mock<IDateTimeProvider> MockDateTimeProvider;

    protected IntegrationTestBase(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        MockDateTimeProvider = Factory.MockDateTimeProvider;
    }

    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
