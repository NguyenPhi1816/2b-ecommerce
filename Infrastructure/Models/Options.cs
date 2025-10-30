using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Options
{
    public Guid Id { get; set; }

    public Guid SpuId { get; set; }

    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual ICollection<OptionValues> OptionValues { get; set; } = new List<OptionValues>();

    public virtual Spu Spu { get; set; } = null!;
}
