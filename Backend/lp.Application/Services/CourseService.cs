using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;


    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository=courseRepository;
    }

    public async Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto)
    {
        var course=new Course
        {
            Title=dto.Title,
            Field=dto.Field,
            Duration=dto.Duration,
            Price=dto.Price
        };

        await _courseRepository.AddCourseAsync(course);
        await _courseRepository.SaveChangesAsync();

        return new CourseResponseDto
        {
            Id=course.Id,
            Title=course.Title,
            Field=course.Field,
            Price=course.Price
        };
    }

    public async Task<CourseResponseDto?> ViewCourseAsync(int id)
    {
        var course=await _courseRepository.ViewCourseById(id);

        if (course == null)
        {
            throw new NotFoundException("Cannot find the course");
        }

        return new CourseResponseDto
        {
            Id=course.Id,
            Title=course.Title,
            Field=course.Field,
            Price=course.Price,
            Duration=course.Duration
        };
    }

    public async Task<List<CourseResponseDto>> ViewAllCourses()
    {
        var courses=await _courseRepository.ViewAllCourses();

        return courses.Select(course=> new CourseResponseDto
        {
           Id=course.Id,
            Title=course.Title,
            Field=course.Field,
            Price=course.Price,
            Duration=course.Duration
        }).ToList();
    }
}