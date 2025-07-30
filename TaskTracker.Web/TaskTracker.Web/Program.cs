using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TaskTracker.Dal;
using TaskTracker.Web;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddRepositories();

#region Configure database context

var connectionString = builder.Configuration.GetConnectionString(name: "Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception(message: "Не найдена строка подключения");
}

builder.Services.ConfigureDbContext(connectionString);

#endregion

builder.Services.AddCustomHealthChecks();

#endregion

#region Configure OpenApi

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<TaskTrackerApiTransformer>();
});

#endregion

var app = builder.Build();

#region Create database and apply migrations

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await dbContext.Database.MigrateAsync();
}

#endregion

#region Configure the HTTP request pipeline

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(url: "/openapi/v1.json", name: "OpenAPI v1");
    });

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicyBuilder
    => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("api/health");

#endregion

app.Run();

/// <summary>
/// Класс Program для возможности его использования в интеграционных тестах
/// </summary>
public partial class Program;
