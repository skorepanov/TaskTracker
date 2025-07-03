using Npgsql;
using Xunit;

namespace TaskTracker.IntegrationTests;

public abstract class IntegrationTestBase
    : IClassFixture<ApiWebApplicationFactory>, IAsyncLifetime
{
    protected readonly ApiWebApplicationFactory Factory;
    protected readonly HttpClient Client;
    protected NpgsqlConnection Connection;

    protected IntegrationTestBase(ApiWebApplicationFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        Connection = factory.CreateConnection();
    }

    public async Task InitializeAsync()
    {
        await Factory.ResetDatabaseAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
