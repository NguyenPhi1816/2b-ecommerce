using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Users
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? RoleId { get; set; }

    public virtual Carts? Carts { get; set; }

    public virtual ICollection<DeliveryInfo> DeliveryInfo { get; set; } = new List<DeliveryInfo>();

    public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    public virtual ICollection<RankUsers> RankUsers { get; set; } = new List<RankUsers>();

    public virtual ICollection<Reviews> Reviews { get; set; } = new List<Reviews>();

    public virtual Roles? Role { get; set; }

    public virtual ICollection<VoucherRedemptions> VoucherRedemptions { get; set; } = new List<VoucherRedemptions>();
}
