using Application.Configurations.Authorization;
using Application.Interfaces;
using Application.Configurations.Routing;
using Application.Services;
using _2b_ecommerce.Application.Configurations.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using _2b_ecommerce.Infrastructure.Persistence;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Load secrets when running locally
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// 1) Logging & configuration guardrails
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Read the connection string from configuration.
// In containers/CI, prefer environment variable: ConnectionStrings__Default
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Missing ConnectionStrings:Default");

// 2) API essentials
builder.Services.AddControllers(options =>
{
    options.Conventions.Insert(0, new ApiPrefixConvention("api/v1"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 3) Configure Npgsql data source and map .NET enums to PostgreSQL enum types
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(
        dataSource,
        npg => npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    )
);

// JWT Bearer
var jwtOpt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;
if (string.IsNullOrWhiteSpace(jwtOpt.Key))
    throw new InvalidOperationException("Missing Jwt:Key. Configure it via user secrets or environment variables.");
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new()
        {
            ValidIssuer = jwtOpt.Issuer,
            ValidAudience = jwtOpt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOpt.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

// 5) Application services (DI)
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

// 6) HTTP pipeline
if (app.Environment.IsDevelopment())
{
    // Swagger UI for local development
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto-apply EF migrations on startup (DEV ONLY).
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

