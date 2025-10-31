using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Payments
{
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; }
}

public partial class PaymentConfiguration : IEntityTypeConfiguration<Payments>
{
    public void Configure(EntityTypeBuilder<Payments> e)
    {
        e.Property(p => p.Method).HasColumnType("\"PaymentMethod\"");
        e.Property(p => p.Status).HasColumnType("\"PaymentStatus\"");
    }
}
