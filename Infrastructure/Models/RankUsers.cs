using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class RankUsers
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid RankId { get; set; }

    public decimal TotalSpent { get; set; }

    public int TotalOrders { get; set; }

    public DateTime LastOrderAt { get; set; }

    public DateTime AchievedAt { get; set; }

    public DateTime ExpiredAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Ranks Rank { get; set; } = null!;

    public virtual Users User { get; set; } = null!;
}
