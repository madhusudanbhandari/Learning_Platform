using lp.Application.Dtos;
using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface ICourseService
{
    Task<CourseResponseDto> CreateCourseAsync(CreateCourseDto dto);

    Task<CourseResponseDto?> ViewCourseAsync(int id);

    Task<List<CourseResponseDto>> ViewAllCourses();

}