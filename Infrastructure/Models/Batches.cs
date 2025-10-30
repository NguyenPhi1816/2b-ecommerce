using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Batches
{
    public Guid Id { get; set; }

    public Guid SkuId { get; set; }

    public string BatchCode { get; set; } = null!;

    public DateOnly? MfgDate { get; set; }

    public DateOnly? ExpDate { get; set; }

    public int OnHand { get; set; }

    public decimal? SellPrice { get; set; }

    public virtual ICollection<InventoryMovements> InventoryMovements { get; set; } = new List<InventoryMovements>();

    public virtual ICollection<PurchaseItems> PurchaseItems { get; set; } = new List<PurchaseItems>();

    public virtual Sku Sku { get; set; } = null!;
}
