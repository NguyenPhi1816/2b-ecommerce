using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Purchases
{
    public Guid Id { get; set; }

    public string? PoNumber { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime PurchasedAt { get; set; }

    public string? RefCode { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<PurchaseItems> PurchaseItems { get; set; } = new List<PurchaseItems>();

    public virtual Suppliers? Supplier { get; set; }
}
