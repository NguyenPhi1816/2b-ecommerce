using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using _2b_ecommerce.Infrastructure.Models;

namespace _2b_ecommerce.Infrastructure.Persistence;

public partial class AppDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<InventoryType>("public", "InventoryType");
        modelBuilder.HasPostgresEnum<PaymentMethod>("public", "PaymentMethod");
        modelBuilder.HasPostgresEnum<PaymentStatus>("public", "PaymentStatus");
        modelBuilder.HasPostgresEnum<DiscountMode>("public", "DiscountMode");
        modelBuilder.HasPostgresEnum<OrderStatus>("public", "OrderStatus");
        modelBuilder.HasPostgresEnum<GenderType>("public", "GenderType");

        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new VoucherConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new InventoryMovementConfiguration());
    }
}
