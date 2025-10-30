using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Roles
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
