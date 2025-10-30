using System;
using System.Collections.Generic;

namespace _2b_ecommerce.Infrastructure.Models;

public partial class RolePermissions
{
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }

    public virtual Permissions Permission { get; set; } = null!;

    public virtual Roles Role { get; set; } = null!;
}
