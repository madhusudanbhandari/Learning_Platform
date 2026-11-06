using System.Security.Claims;
using FluentValidation;
using lp.Application.Dtos;
using lp.Application.Interface;
using lp.Application.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp.Api.Controller;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CourseController:ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IValidator<CreateCourseDto> _validator;

    public CourseController(ICourseService courseService,IValidator<CreateCourseDto> validator
    )
    {
        _courseService=courseService;
        _validator=validator;
    }

    [Authorize(Roles ="Teacher")]
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto)
    {
        var validationResult=await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            var errors=validationResult.Errors
                        .GroupBy(e=>e.PropertyName)
                        .ToDictionary(
                            g=>g.Key,
                            g=>g.Select(e=>e.ErrorMessage).ToArray()
                        );

            var problemDetails=new ValidationProblemDetails(errors)
            {
                Status=StatusCodes.Status400BadRequest,
                Title="Validation Failed"
            };

            return BadRequest(problemDetails);

        }

       

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