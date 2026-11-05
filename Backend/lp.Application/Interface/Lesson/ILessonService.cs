using lp.Application.Dtos;

namespace lp.Application.Interface;

public interface ILessonService
{
    Task<ViewLessonDdto> CreateLesson(int teacherId,CreateLesson lesson);
    Task<ViewLessonDdto> GetLessonById(int id);
    Task<List<ViewLessonDdto>> GetAllLessonsByCourse(int courseId);
}