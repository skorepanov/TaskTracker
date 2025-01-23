using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace TaskTracker.Web.HealthChecks;

/// <summary>
/// Проверка доступности БД
/// </summary>
public class DataBaseHealthCheck(IConfiguration _configuration)
    : IHealthCheck
{
    /// <summary>
    /// Проверить доступность БД
    /// </summary>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString(name: "Default");

        await using var connection = new NpgsqlConnection(connectionString);

        try
        {
            await connection.OpenAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(exception: exception);
        }
    }
}