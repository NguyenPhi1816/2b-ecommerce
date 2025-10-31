using System;
using System.Collections.Generic;
using Domain.Enums;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Vouchers
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public DateTime? StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public decimal Value { get; set; }

    public decimal? MaxDiscount { get; set; }

    public decimal? MinOrderTotal { get; set; }

    public int? TotalLimit { get; set; }

    public int? PerCustomerLimit { get; set; }

    public bool IsActive { get; set; }

    public DiscountMode Mode { get; set; }

    public virtual ICollection<VoucherRedemptions> VoucherRedemptions { get; set; } = new List<VoucherRedemptions>();
}
