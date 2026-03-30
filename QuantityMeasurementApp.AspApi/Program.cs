using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.AspApi.Middleware;
using QuantityMeasurementApp.AspApi.Security;
using QuantityMeasurementApp.AspApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title       = "Quantity Measurement API",
        Version     = "v1",
        Description = "UC17 - EF Core + SQL Server + Redis + JWT + AES + BCrypt"
    });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization", Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer", BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Paste your JWT token here."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// EF Core - SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)));

// Repositories
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Business service
builder.Services.AddSingleton<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

// Security
builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<AesEncryptionService>();

// JWT
var jwtKey = builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("Jwt:SecretKey missing.");

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, ValidateAudience = true,
        ValidateLifetime = true, ValidateIssuerSigningKey = true,
        ValidIssuer    = builder.Configuration["Jwt:Issuer"],
        ValidAudience  = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

// Redis - abortConnect=false so app NEVER crashes if Redis is offline
string redisConn = builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379";
var redisConfig  = ConfigurationOptions.Parse(redisConn);
redisConfig.AbortOnConnectFail  = false;
redisConfig.ConnectTimeout      = 3000;
redisConfig.SyncTimeout         = 3000;
redisConfig.ReconnectRetryPolicy = new LinearRetry(5000);

var redisMultiplexer = await ConnectionMultiplexer.ConnectAsync(redisConfig);
builder.Services.AddSingleton<IConnectionMultiplexer>(redisMultiplexer);
builder.Services.AddSingleton<RedisTokenService>();

bool redisOnline = redisMultiplexer.IsConnected;
Console.WriteLine(redisOnline
    ? $"[Redis] Connected to {redisConn}"
    : "[Redis] Offline - token blacklisting and rate limiting disabled.");

builder.Services.AddHealthChecks();
builder.Services.AddCors(o =>
    o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();


// Skip migrations when running inside WebApplicationFactory tests
if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Test")
{
    using var migrationScope = app.Services.CreateScope();
    var migrationDb = migrationScope.ServiceProvider.GetRequiredService<AppDbContext>();
    migrationDb.Database.Migrate();
    Console.WriteLine("[EF Core] Migrations applied successfully.");
}

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

Console.WriteLine("==================================================");
Console.WriteLine("   QUANTITY MEASUREMENT API  (UC17)");
Console.WriteLine("==================================================");
Console.WriteLine("  Database   : SQL Server (EF Core + Migrations)");
Console.WriteLine("  Auth       : JWT Bearer");
Console.WriteLine("  Hashing    : BCrypt WorkFactor=12 + explicit salt");
Console.WriteLine("  Encryption : AES-256-CBC");
Console.WriteLine($"  Redis      : {(redisOnline ? "Connected" : "Offline")}");
Console.WriteLine("  Swagger UI : http://localhost:5271/swagger");
Console.WriteLine("  Health     : http://localhost:5271/health");
Console.WriteLine("==================================================");

app.Run();

public partial class Program { }