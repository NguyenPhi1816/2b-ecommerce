using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class OrderItems
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid SkuId { get; set; }

    public Guid UnitId { get; set; }

    public Guid DiscountId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }

    public virtual Discounts Discount { get; set; } = null!;

    public virtual Orders Order { get; set; } = null!;

    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();

    public virtual Sku Sku { get; set; } = null!;

    public virtual Units Unit { get; set; } = null!;
}
