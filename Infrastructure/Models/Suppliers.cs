using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Suppliers
{
    public Guid Id { get; set; }

    public string? SupplierCode { get; set; }

    public string Name { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Line { get; set; }

    public string? Ward { get; set; }

    public string? Province { get; set; }

    public string? Country { get; set; }

    public bool IsActive { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<Purchases> Purchases { get; set; } = new List<Purchases>();
}
