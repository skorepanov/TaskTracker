using Microsoft.OpenApi.Models;
using TaskTracker.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddRepositories();

var connectionString = builder.Configuration.GetConnectionString(name: "Default");
builder.Services.ConfigureDbContext(connectionString);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
