using System.Security.Claims;
using lp.Application.Dtos;
using lp.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp.Api.Controller;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CourseController:ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService=courseService;
    }

    [Authorize(Roles ="Teacher")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
    {
        var user=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(user,out int teacherId))
        {
            return Unauthorized();
        }
        var course=await _courseService.CreateCourseAsync(teacherId,dto);

        return Ok(course);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ViewCourse(int id)
    {
        var course=await _courseService.ViewCourseAsync(id);

        return Ok(course);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ViewAllCourses()
    {
        var courses=await _courseService.ViewAllCourses();

        return Ok(courses);

    }
}