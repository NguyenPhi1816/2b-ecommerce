using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class DeliveryInfo
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Line { get; set; }

    public string? Ward { get; set; }

    public string? Province { get; set; }

    public string? Country { get; set; }

    public string? Status { get; set; }

    public bool IsDefault { get; set; }

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    public virtual Users User { get; set; } = null!;
}
