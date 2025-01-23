using Microsoft.OpenApi.Models;
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

builder.Services
    .AddHealthChecks()
    .AddNpgSql(connectionString);

#endregion

#region Add Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(name: "v1", new OpenApiInfo
    {
        Version = "0.0.1",
        Title = "Task Tracker"
    });

    var baseDirectory = AppContext.BaseDirectory;
    var xmlCommentsPath = Path.Combine(baseDirectory, "TaskTrackerAPI.xml");
    options.IncludeXmlComments(xmlCommentsPath);
});

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health");

#endregion

app.Run();
