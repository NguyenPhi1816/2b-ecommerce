using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class CartItems
{
    public Guid Id { get; set; }

    public Guid CartId { get; set; }

    public Guid SkuId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Carts Cart { get; set; } = null!;

    public virtual Sku Sku { get; set; } = null!;
}
