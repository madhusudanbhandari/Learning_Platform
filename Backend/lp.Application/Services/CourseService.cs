using AutoMapper;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;


    public CourseService(ICourseRepository courseRepository,IMapper mapper)
    {
        _courseRepository=courseRepository;
        _mapper=mapper;
    }

    public async Task<CourseResponseDto> CreateCourseAsync(int teacherId,CreateCourseDto dto)
    {
        var course=new Course
        {
            Title=dto.Title,
            Field=dto.Field,
            Duration=dto.Duration,
            Price=dto.Price,
            CreatedBy=teacherId
        };

        await _courseRepository.AddCourseAsync(course);
        await _courseRepository.SaveChangesAsync();

        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<CourseResponseDto?> ViewCourseAsync(int id)
    {
        var course=await _courseRepository.ViewCourseById(id);

        if (course == null)
        {
            throw new NotFoundException("Cannot find the course");
        }

        return _mapper.Map<CourseResponseDto>(course);
    }

    public async Task<List<CourseResponseDto>> ViewAllCourses()
    {
        var courses=await _courseRepository.ViewAllCourses();

        return _mapper.Map<List<CourseResponseDto>>(courses);
    }
}