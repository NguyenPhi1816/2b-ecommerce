using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2b_ecommerce.Infrastructure.Models;
public partial class InventoryMovement
{
    public InventoryType Type { get; set; }
}
public partial class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovement>
{
    public void Configure(EntityTypeBuilder<InventoryMovement> e)
    {
        e.Property(p => p.Type).HasColumnType("inventory_type");
    }
}
