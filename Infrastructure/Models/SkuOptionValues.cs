using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class SkuOptionValues
{
    public Guid Id { get; set; }

    public Guid ValueId { get; set; }

    public Guid SkuId { get; set; }

    public virtual Sku Sku { get; set; } = null!;

    public virtual OptionValues Value { get; set; } = null!;
}
