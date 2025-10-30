using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class VoucherRedemptions
{
    public Guid Id { get; set; }

    public Guid VoucherId { get; set; }

    public Guid UserId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime RedeemedAt { get; set; }

    public virtual Orders Order { get; set; } = null!;

    public virtual Users User { get; set; } = null!;

    public virtual Vouchers Voucher { get; set; } = null!;
}
