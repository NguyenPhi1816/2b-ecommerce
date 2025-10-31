using Application.DTOs.Pagination;
using Application.DTOs.Users;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _2b_ecommerce.Api.Controllers;

[ApiController]
[Route("users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _users;
    public UsersController(IUserService users) => _users = users;

    [Authorize(Policy = "perm:users:read")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        [FromQuery, Range(1, int.MaxValue)] int page = 1,
        [FromQuery, Range(1, 200)] int pageSize = 20,
        CancellationToken ct = default)
    {
        var filter = new UserFilter { Search = search, IsActive = isActive };
        var result = await _users.GetUsersAsync(filter, page, pageSize, ct);

        return Ok(new
        {
            statusCode = 200,
            message = "Get users success",
            data = result.Items,
            pagination = new
            {
                result.Page,
                result.PageSize,
                result.Total,
                result.TotalPages
            }
        });
    }

    [Authorize(Policy = "perm:users:read")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(id, ct);
        if (user is null)
            return NotFound(new { statusCode = 404, message = "User not found" });

        return Ok(new { statusCode = 200, message = "OK", data = user });
    }
}
