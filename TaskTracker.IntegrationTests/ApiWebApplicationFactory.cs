using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Npgsql;
using Respawn;
using TaskTracker.Bll;
using TaskTracker.Dal;
using Xunit;

namespace TaskTracker.IntegrationTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public Mock<IDateTimeProvider> MockDateTimeProvider { get; private set; }

    private readonly IConfiguration _configuration;
    private NpgsqlConnection _connection;
    private readonly string _connectionString;
    private Respawner _respawner;

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

            MockDateTimeProvider = new Mock<IDateTimeProvider>();
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
        await _respawner.ResetAsync(_connection);
    }

    public new async Task DisposeAsync()
    {
        await _connection.CloseAsync();
        await _connection.DisposeAsync();
        await base.DisposeAsync();
    }
}