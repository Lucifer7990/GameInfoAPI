using DotNetEnv;
using Microsoft.EntityFrameworkCore;

Env.Load(); // loads .env file
var dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION");
if (string.IsNullOrEmpty(dbConnection))
{
    throw new InvalidOperationException("DB_CONNECTION environment variable is not set.");
}
var uri = new Uri(dbConnection);
var userInfo = uri.UserInfo.Split(':');

var connectionString = $"Host={uri.Host};Port=5432;Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
