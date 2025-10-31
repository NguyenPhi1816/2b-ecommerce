using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Vouchers
{
    public DiscountMode Mode { get; set; }
}
public partial class VoucherConfiguration : IEntityTypeConfiguration<Vouchers>
{
    public void Configure(EntityTypeBuilder<Vouchers> e)
    {
        e.Property(p => p.Mode).HasColumnType("\"DiscountMode\"");
    }
}
