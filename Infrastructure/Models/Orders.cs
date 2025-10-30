using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Orders
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid DeliveryInfoId { get; set; }

    public decimal ProductDiscount { get; set; }

    public decimal VoucherDiscount { get; set; }

    public decimal ShippingFee { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public virtual DeliveryInfo DeliveryInfo { get; set; } = null!;

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();

    public virtual Users User { get; set; } = null!;

    public virtual ICollection<VoucherRedemptions> VoucherRedemptions { get; set; } = new List<VoucherRedemptions>();
}
