using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class SkuDiscounts
{
    public Guid Id { get; set; }

    public Guid DiscountId { get; set; }

    public Guid SkuId { get; set; }

    public virtual Discounts Discount { get; set; } = null!;

    public virtual Sku Sku { get; set; } = null!;
}
