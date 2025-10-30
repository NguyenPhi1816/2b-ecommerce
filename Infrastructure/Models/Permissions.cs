using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class Permissions
{
    public Guid Id { get; set; }

    public string? Resource { get; set; }

    public string? Action { get; set; }

    public virtual ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
}
