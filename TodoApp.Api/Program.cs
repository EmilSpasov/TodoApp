using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoApp.Core.Interfaces;
using TodoApp.Core.Mappers;
using TodoApp.Infrastructure.Data;
using TodoApp.Infrastructure.Repositories;
using TodoApp.Infrastructure.Services;
using TodoApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ApplyMigrations(app);

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    });

    // If you want to use in-memory database instead of SQLite,
    // comment out the SQLite configuration and uncomment the in-memory database configuration
    var dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
    var dbFilePath = Path.Combine(dataDirectory, "TodoApp.db");
    var connectionString = $"Data Source={dbFilePath}";
    Console.WriteLine($"Connection string: {connectionString}");

    services.AddDbContext<TodoContext>(options =>
        options.UseSqlite(connectionString));

    // In-memory database configuration
    // services.AddDbContext<TodoContext>(options =>
    //     options.UseInMemoryDatabase("InMemoryDb"));

    services.AddScoped<ITaskRepository, TaskRepository>();
    services.AddScoped<ITaskService, TaskService>();
    services.AddTransient<ITaskMapper, TaskMapper>();
}

void ApplyMigrations(WebApplication app)
{
    var dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");

    if (!Directory.Exists(dataDirectory))
    {
        Directory.CreateDirectory(dataDirectory);
    }

    try
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TodoContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database migration applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}


void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
}
