using Microsoft.AspNetCore.Authorization;

namespace Application.Configurations.Authorization;

public sealed record PermissionRequirement(string Permission) : IAuthorizationRequirement;
