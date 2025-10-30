using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Carts
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    public virtual Users User { get; set; } = null!;
}
