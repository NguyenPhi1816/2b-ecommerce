using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class SpuImages
{
    public Guid Id { get; set; }

    public Guid SpuId { get; set; }

    public string Url { get; set; } = null!;

    public string? Alt { get; set; }

    public int SortOrder { get; set; }

    public virtual Spu Spu { get; set; } = null!;
}
