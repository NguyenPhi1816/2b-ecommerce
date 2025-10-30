using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class PurchaseItems
{
    public Guid Id { get; set; }

    public Guid PurchaseId { get; set; }

    public Guid BatchId { get; set; }

    public Guid UnitId { get; set; }

    public int Quantity { get; set; }

    public decimal? ImportPrice { get; set; }

    public decimal? UnitConversionFactor { get; set; }

    public virtual Batches Batch { get; set; } = null!;

    public virtual Purchases Purchase { get; set; } = null!;

    public virtual Units Unit { get; set; } = null!;
}
