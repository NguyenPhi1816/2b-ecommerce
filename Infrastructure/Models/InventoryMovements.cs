using System;
using System.Collections.Generic;
using Domain.Enums;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class InventoryMovements
{
    public Guid Id { get; set; }

    public Guid BatchId { get; set; }

    public Guid UnitId { get; set; }

    public Guid? RelatedOrderId { get; set; }

    public Guid? RelatedPurchaseId { get; set; }
    
    public InventoryType Type { get; set; }

    public decimal Quantity { get; set; }

    public DateTime MovedAt { get; set; }

    public string? Note { get; set; }

    public virtual Batches Batch { get; set; } = null!;

    public virtual Units Unit { get; set; } = null!;
}
