using _2b_ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Npgsql;
using Application.Interfaces;
using Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
var connString = builder.Configuration.GetConnectionString("Default");
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);

// Mapping Enums
dataSourceBuilder.MapEnum<InventoryType>("inventory_type");
dataSourceBuilder.MapEnum<PaymentMethod>("payment_method");
dataSourceBuilder.MapEnum<PaymentStatus>("payment_status");
dataSourceBuilder.MapEnum<DiscountMode>("discount_mode");
dataSourceBuilder.MapEnum<OrderStatus>("order_status");
dataSourceBuilder.MapEnum<GenderType>("gender_type");

var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(dataSource));

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Nếu bạn dùng HTTPS redirect, nên chỉ bật ở Development
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// AuthZ nếu có
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ==== Auto-migrate on startup ====
// using (var scope = app.Services.CreateScope())
// {
//     try
//     {
//         var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//         db.Database.Migrate(); // tạo DB & áp dụng migrations vào container Postgres
//     }
//     catch (Exception ex)
//     {
//         Console.WriteLine($"[DB Migrate] {ex.Message}");
//         // tùy bạn: có thể rethrow để fail fast
//     }
// }

app.Run();
