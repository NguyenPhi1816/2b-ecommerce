using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Spu
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? SpuCode { get; set; }

    public Guid? CategoryId { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Categories? Category { get; set; }

    public virtual ICollection<Options> Options { get; set; } = new List<Options>();

    public virtual ICollection<Sku> Sku { get; set; } = new List<Sku>();

    public virtual ICollection<SpuImages> SpuImages { get; set; } = new List<SpuImages>();
}
