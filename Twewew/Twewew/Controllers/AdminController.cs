using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twewew.DTOs;
using Twewew.Requests.Auth;
using Twewew.Requests.User;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    [HttpHead]
    public async Task<ActionResult<List<UserDto>>> GetUsersAsync()
    {
        var response = await _adminService.GetAsync();

        return Ok(response);
    }

    [HttpGet("{id:int:min(1)}", Name = nameof(GetUserByIdAsync))]
    public async Task<ActionResult<UserDto>> GetUserByIdAsync(UserRequest request)
    {
        var response = await _adminService.GetUserById(request);

        return Ok(response);
    }

    [HttpPost("add-role")]
    public async Task<IActionResult> AddRole(string role)
    {
        await _adminService.AddRole(role);

        return Ok($"{role} role was added successfully");
        ;
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(UserRoleRequest request)
    {
        await _adminService.AssignRole(request);

        return Ok($"{request.Username} username was assigned successfully to {request.Role} role.");
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<ActionResult> UpdateASync(
        [FromRoute] Guid id,
        [FromBody] UpdateUserRequest request)
    {
        await _adminService.UpdateAsync(request);

        return NoContent();
    }

}
