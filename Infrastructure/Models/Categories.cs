using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Categories
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public Guid? ParentId { get; set; }

    public virtual ICollection<Categories> InverseParent { get; set; } = new List<Categories>();

    public virtual Categories? Parent { get; set; }

    public virtual ICollection<Spu> Spu { get; set; } = new List<Spu>();
}
