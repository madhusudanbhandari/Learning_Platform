using lp.Application.Interface;
using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lp.Infrastructure.Repository;

public class LessonRepository:ILessonRepository
{
    private readonly LpDbConext _context;

    public LessonRepository(LpDbConext context)
    {
        _context=context;
    }

    public async Task<Lesson?> GetLessonById(int id)
    {
        return await _context.Lessons.FirstOrDefaultAsync(l=>l.Id==id);
    }

    public async Task<List<Lesson>> GetLessonByCourse(int id)
    {
        return await _context.Lessons
                                .Where(l=>l.CourseId==id)
                                .Include(l=>l.Course)
                                .ToListAsync();

    }

    public async Task AddAsync(Lesson lesson)
    {
         await _context.Lessons.AddAsync(lesson);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}