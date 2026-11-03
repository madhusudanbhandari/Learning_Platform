using System.Security.Claims;
using lp.Application.Dtos;
using lp.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp.Api.Controller;

[ApiController]
[Route("api/requests")]
[Authorize]
public class EnrollmentController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentController(IEnrollmentService enrollmentService)
    {
        _enrollmentService=enrollmentService;
    }

    [Authorize(Roles ="Student")]
    [HttpPost]
    public async Task<IActionResult>  EnrollmentRequest(EnrollRequestDto dto)
    {
        var user=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(user,out int userId))
        {
            return Unauthorized();
        }

        var request=await _enrollmentService.RequestEnrollment(userId,dto);

        return Ok(request);
    }

    [Authorize(Roles ="Teacher")]
    [HttpGet]
    public async Task<IActionResult> ViewEnrollRequests(int courseId)
    {
        var user=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(user,out int teacherId))
        {
            return Unauthorized();
        }

        var requests=await _enrollmentService.ViewEnrollRequests(teacherId,courseId);

        return Ok(requests);
    }

    [HttpPatch("status")]
    [Authorize(Roles ="Teacher")]
    public async Task<IActionResult> VerifyRequest(int reqId,UpdateRequestDto dto)
    {
        var user=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(user,out int teacherId))
        {
            return Unauthorized();
        }

        var request=await _enrollmentService.VerifyRequest(teacherId,reqId,dto);

        return Ok(request);
    }
}