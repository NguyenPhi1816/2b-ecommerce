using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Units
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<InventoryMovements> InventoryMovements { get; set; } = new List<InventoryMovements>();

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<PurchaseItems> PurchaseItems { get; set; } = new List<PurchaseItems>();

    public virtual ICollection<Sku> Sku { get; set; } = new List<Sku>();
}
