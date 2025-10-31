using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class InventoryMovements
{
    public InventoryType Type { get; set; }
}
public partial class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovements>
{
    public void Configure(EntityTypeBuilder<InventoryMovements> e)
    {
        e.Property(p => p.Type).HasColumnType("\"InventoryType\"");
    }
}
