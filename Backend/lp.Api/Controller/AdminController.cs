using lp.Application.Dtos;
using lp.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp.Api.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService=adminService;
    }

    [Authorize(Roles ="Admin")]
    [HttpGet("Pending-users")]
    public async Task<IActionResult> ViewPendingUsers()
    {
        var users=await _adminService.ViewPendingUsers();

        return Ok(users);
    }

    [Authorize(Roles ="Admin")]
    [HttpPatch("verify-users")]
    public async Task<IActionResult> VerifyUsers(int id, VerifyUserDto dto)
    {
        var user=await _adminService.VerifyUserAsync(id,dto);

        return Ok(user);
    }

}