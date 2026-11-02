using lp.Application.Dtos;
using lp.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace lp.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService=userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        var user=await _userService.RegisterAsync(request);

        return Ok(user);
    }
    [EnableRateLimiting("LoginPolicy")]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var loggedIn=await _userService.LoginAsync(request);
        return Ok(loggedIn);
    }
}