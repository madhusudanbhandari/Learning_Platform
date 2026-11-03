using lp.Application.Interface;
using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lp.Infrastructure.Repository;

public class CourseRepository : ICourseRepository
{
    private readonly LpDbConext _context;

    public CourseRepository(LpDbConext context)
    {
        _context=context;
    }

    public async Task<Course?> ViewCourseById(int id)
    {
        return await _context.Courses.FirstOrDefaultAsync(c=>c.Id==id);
    }
    

    public async Task<List<Course>> ViewAllCourses()
    {
        return await _context.Courses.ToListAsync();
    }

    public async Task AddCourseAsync(Course course)
    {
         await _context.Courses.AddAsync(course);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}