using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Discounts
{
    public DiscountMode Mode { get; set; }
}
public partial class DiscountConfiguration : IEntityTypeConfiguration<Discounts>
{
    public void Configure(EntityTypeBuilder<Discounts> e)
    {
        e.Property(p => p.Mode).HasColumnType("\"DiscountMode\"");
    }
}
