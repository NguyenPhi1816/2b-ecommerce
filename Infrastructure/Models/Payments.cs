using System;
using System.Collections.Generic;
using Domain.Enums;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Payments
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaidAt { get; set; }

    public string? Provider { get; set; }

    public string? TransactionRef { get; set; }

    public string? ProviderTxnId { get; set; }

    public string? RefundRef { get; set; }

    public string? OrderCodeSnapshot { get; set; }

    public string? RawPayload { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Orders Order { get; set; } = null!;

    public PaymentMethod Method { get; set; }
    
    public PaymentStatus Status { get; set; }
}
