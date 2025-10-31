using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Orders
{
    public OrderStatus Status { get; set; }          
    public PaymentMethod PaymentMethod { get; set; } 
}
public partial class OrderConfiguration : IEntityTypeConfiguration<Orders>
{
    public void Configure(EntityTypeBuilder<Orders> e)
    {
        e.Property(p => p.Status).HasColumnType("\"OrderStatus\"");
        e.Property(p => p.PaymentMethod).HasColumnType("\"PaymentMethod\"");
    }
}
