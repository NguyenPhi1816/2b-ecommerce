using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Discount
{
    public DiscountMode Mode { get; set; }
}
public partial class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> e)
    {
        e.Property(p => p.Mode).HasColumnType("discount_mode");
    }
}
