using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

Env.Load(); // loads .env file 
var dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbConnection));

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    var isProduction = app.Environment.IsProduction();

    options.Servers = isProduction
        ? new[] { new ScalarServer("https://gameinfoapi.onrender.com") }
        : null; // Let Scalar auto-detect locally
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
