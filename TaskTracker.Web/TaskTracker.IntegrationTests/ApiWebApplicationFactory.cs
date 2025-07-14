using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;

namespace TaskTracker.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public readonly Mock<IDateTimeProvider> MockDateTimeProvider;

    private readonly IConfiguration _configuration;
    private NpgsqlConnection? _connection;
    private readonly string _connectionString;
    private Respawner? _respawner;

    public ApiWebApplicationFactory()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("integrationSettings.json")
            .Build();

        var connectionString = _configuration.GetConnectionString("Default");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception(message: "Не найдена строка подключения");
        }

        _connectionString = connectionString;

        MockDateTimeProvider = new Mock<IDateTimeProvider>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(_configuration);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationContext>>();
            services.RemoveAll<IDateTimeProvider>();

            services.AddDbContext<ApplicationContext>(options =>
            {
                options
                    .UseNpgsql(_connectionString)
                    .UseSnakeCaseNamingConvention();
            });

            services.AddSingleton(MockDateTimeProvider.Object);
        });
    }

    public async Task InitializeAsync()
    {
        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        await dbContext.Database.MigrateAsync();

        var respawnerOptions = new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = ["__EFMigrationsHistory"],
            WithReseed = true,
        };

        _respawner = await Respawner.CreateAsync(_connection, respawnerOptions);
    }

    public async Task ResetDatabaseAsync()
    {
        if (_connection is not null && _respawner is not null)
        {
            await _respawner.ResetAsync(_connection);
        }
    }

    public new async Task DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        await base.DisposeAsync();
    }
}
