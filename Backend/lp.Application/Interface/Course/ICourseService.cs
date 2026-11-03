using lp.Application.Dtos;
using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface ICourseService
{
    Task<CourseResponseDto> CreateCourseAsync(int teacherId,CreateCourseDto dto);

    Task<CourseResponseDto?> ViewCourseAsync(int id);

    Task<List<CourseResponseDto>> ViewAllCourses();

}