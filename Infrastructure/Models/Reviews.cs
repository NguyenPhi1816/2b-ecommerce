using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Reviews
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid SkuId { get; set; }

    public Guid OrderItemId { get; set; }

    public int Rating { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual OrderItems OrderItem { get; set; } = null!;

    public virtual Sku Sku { get; set; } = null!;

    public virtual Users User { get; set; } = null!;
}
