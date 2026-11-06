using System.Security.Claims;
using lp.Application.Dtos;
using lp.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp.Api.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService=lessonService;
    }

    [Authorize(Roles ="Teacher")]
    [HttpPost]
    public async Task<IActionResult> CreateLesson(CreateLesson lesson)
    {
        var teacher=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(teacher,out int teacherId))
        {
            return Unauthorized();
        }

        var newLesson=await _lessonService.CreateLesson(teacherId,lesson);

        return Ok(newLesson);
    }

    [Authorize]
    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetLesson(int lesson)
    {
        
        var newLesson=await _lessonService.GetLessonById(lesson);

        return Ok(newLesson);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllLessons(int courseId)
    {
        var newLessons=await _lessonService.GetAllLessonsByCourse(courseId);

        return Ok(newLessons);
    }
}