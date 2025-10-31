using _2b_ecommerce.Infrastructure.Persistence;
using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

//
// 1) Logging & configuration guardrails
//
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Read the connection string from configuration.
// In containers/CI, prefer environment variable: ConnectionStrings__Default
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Missing ConnectionStrings:Default");

//
// 2) API essentials
//
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
// 3) Configure Npgsql and map .NET enums to PostgreSQL enum types (runtime binding)
//    - MapEnum<T>("type_name") wires the enum at the driver level.
//    - EF still needs HasPostgresEnum<T>() in OnModelCreating to generate CREATE TYPE during migrations.
//
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.MapEnum<InventoryType>("inventory_type");
dataSourceBuilder.MapEnum<PaymentMethod>("payment_method");
dataSourceBuilder.MapEnum<PaymentStatus>("payment_status");
dataSourceBuilder.MapEnum<DiscountMode>("discount_mode");
dataSourceBuilder.MapEnum<OrderStatus>("order_status");
dataSourceBuilder.MapEnum<GenderType>("gender_type");

var dataSource = dataSourceBuilder.Build();

//
// 4) Register DbContext with the configured data source
//    - MigrationsAssembly ensures migrations live with the AppDbContext assembly (Infrastructure.Persistence).
//
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(
        dataSource,
        npg => npg.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
    )
);

//
// 5) Application services (DI)
//
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

//
// 6) HTTP pipeline
//
if (app.Environment.IsDevelopment())
{
    // Swagger UI for local development
    app.UseSwagger();
    app.UseSwaggerUI();

    // Auto-apply EF migrations on startup (DEV ONLY).
    // In production, prefer deploying idempotent SQL scripts with backup/rollback plan.
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
else
{
    // For production, enable Swagger only behind auth or an allowlist if needed.
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

// If running behind a reverse proxy (Nginx/Traefik), consider:
// app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
