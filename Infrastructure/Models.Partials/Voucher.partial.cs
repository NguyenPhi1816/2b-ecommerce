using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class Voucher
{
    public DiscountMode Mode { get; set; }
}
public partial class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
{
    public void Configure(EntityTypeBuilder<Voucher> e)
    {
        e.Property(p => p.Mode).HasColumnType("discount_mode");
    }
}
