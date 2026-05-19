using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Services;
using Infrastructure.Common;
using Microsoft.AspNetCore.HttpOverrides;

Env.Load(); // loads .env file 
var dbConnection = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? throw new InvalidOperationException("DB_CONNECTION is Required");
var SecrateJWTKey = Environment.GetEnvironmentVariable("SECRATE_KEY") ?? throw new InvalidOperationException("SECRATE_KEY is Required");


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageSender, EmailSender>();
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProfileService, ProfileService>();


// AppDomain.CurrentDomain.GetAssemblies() tells AutoMapper to scan all layers/projects for mapping profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(dbConnection));
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();


// 1. Add Authentication Services
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecrateJWTKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Read the token from cookie
            var token = context.Request.Cookies["authToken"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;   // ← Give token to JWT middleware
            }
            return Task.CompletedTask;
        }
    };
});

// builder.Services.AddAuthentication();


var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    var isProduction = app.Environment.IsProduction();

    options.Servers = isProduction
        ? new[] { new ScalarServer("https://gameinfoapi.onrender.com"), new ScalarServer("http://gameinfoapi.onrender.com") }
        : null; // Let Scalar auto-detect locally
});

// 2. Enable Middleware (Order matters!)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Update Migrations to the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
