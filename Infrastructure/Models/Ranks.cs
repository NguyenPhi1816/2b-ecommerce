using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Ranks
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal MinOrderValue { get; set; }

    public decimal MaxOrderValue { get; set; }

    public string Color { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public string CalculationWindow { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<RankUsers> RankUsers { get; set; } = new List<RankUsers>();
}
