namespace TaskTracker.IntegrationTests.Tests;

/// <summary>
/// Класс нужен только для применения атрибута [CollectionDefinition]
/// </summary>
[CollectionDefinition("IntegrationTests")]
public class IntegrationTestCollection : ICollectionFixture<ApiWebApplicationFactory>;

[Collection("IntegrationTests")]
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
