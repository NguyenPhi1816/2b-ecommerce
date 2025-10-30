using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Payment
{
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
}

public partial class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> e)
    {
        e.Property(p => p.Method).HasColumnType("payment_method");
        e.Property(p => p.Status).HasColumnType("payment_status");
    }
}
