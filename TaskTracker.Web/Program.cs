using Scalar.AspNetCore;
using TaskTracker.Web;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddRepositories();

var connectionString = builder.Configuration.GetConnectionString(name: "Default");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception(message: "Не найдена строка подключения");
}

builder.Services.ConfigureDbContext(connectionString);

builder.Services.AddCustomHealthChecks();

#endregion

#region Configure OpenApi

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<TaskTrackerApiTransformer>();
});

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline

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
