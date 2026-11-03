using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface ICourseRepository
{
    public Task<Course?> ViewCourseById(int id);
    public Task<List<Course>> ViewAllCourses();
    public Task AddCourseAsync(Course course);
    public Task SaveChangesAsync();

}