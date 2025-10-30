using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class OptionValues
{
    public Guid Id { get; set; }

    public Guid OptionId { get; set; }

    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual Options Option { get; set; } = null!;

    public virtual ICollection<SkuOptionValues> SkuOptionValues { get; set; } = new List<SkuOptionValues>();
}
