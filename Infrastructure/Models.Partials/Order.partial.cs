using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Order
{
    public OrderStatus Status { get; set; }          
    public PaymentMethod PaymentMethod { get; set; } 
}
public partial class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> e)
    {
        e.Property(p => p.Status).HasColumnType("order_status");
        e.Property(p => p.PaymentMethod).HasColumnType("payment_method");
    }
}
