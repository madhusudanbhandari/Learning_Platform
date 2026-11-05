using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface ILessonRepository
{
    public Task<Lesson?> GetLessonById(int lesson);
    public Task<List<Lesson>> GetLessonByCourse(int courseId);
    public Task AddAsync(Lesson lesson);

    public Task SaveChangesAsync();
}