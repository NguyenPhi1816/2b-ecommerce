using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using _2b_ecommerce.Infrastructure.Models;

namespace _2b_ecommerce.Infrastructure.Persistence;

public partial class AppDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<InventoryType>("public", "inventory_type");
        modelBuilder.HasPostgresEnum<PaymentMethod>("public", "payment_method");
        modelBuilder.HasPostgresEnum<PaymentStatus>("public", "payment_status");
        modelBuilder.HasPostgresEnum<DiscountMode>("public", "discount_mode");
        modelBuilder.HasPostgresEnum<OrderStatus>("public", "order_status");
        modelBuilder.HasPostgresEnum<GenderType>("public", "gender_type");

        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new VoucherConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryMovementConfiguration());
    }
}
