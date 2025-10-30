using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Discounts
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public string? CustomerType { get; set; }

    public string? CustomerTier { get; set; }

    public decimal Value { get; set; }

    public decimal? MaxDiscount { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<SkuDiscounts> SkuDiscounts { get; set; } = new List<SkuDiscounts>();
}
