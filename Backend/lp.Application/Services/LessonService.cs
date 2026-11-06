using FluentValidation;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class LessonService : ILessonService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseRepository _courseRepository;
    public LessonService(ILessonRepository lessonRepository,ICourseRepository courseRepository)
    {
        _lessonRepository=lessonRepository;
        _courseRepository=courseRepository;
    }

    public async Task<ViewLessonDdto> CreateLesson(int teacherId,CreateLesson createLesson)
    {  
        var course=await _courseRepository.ViewCourseById(createLesson.CourseId);

        if (course == null)
        {
            throw new NotFoundException("Cannot find the course");
        }

        var courseTeacher=course.CreatedBy;

        if (courseTeacher != teacherId)
        {
            throw new ForbiddenException("You cannot create lesson for others course");
        }

        var lesson=new Lesson
        {
            CourseId=createLesson.CourseId,
            Title=createLesson.Title,
            
        };

        await _lessonRepository.AddAsync(lesson);
        await _lessonRepository.SaveChangesAsync();

        return new ViewLessonDdto
        {
            Id=lesson.Id,
            CourseId=lesson.CourseId,
            Title=lesson.Title,
        };
    }

    public async Task<ViewLessonDdto> GetLessonById(int id)
    {   
    
        var lesson=await _lessonRepository.GetLessonById(id);

        if (lesson == null)
        {
            throw new NotFoundException("Cannot find the lesson");
        }

        return new ViewLessonDdto
        {
            Id=lesson.Id,
            CourseId=lesson.CourseId,
            Title=lesson.Title,
        };
    }

    public async Task<List<ViewLessonDdto>> GetAllLessonsByCourse(int courseId)
    {
        var course=await _courseRepository.ViewCourseById(courseId);

        if (course == null)
        {
            throw new NotFoundException("cannot find the course");
        }
        var lessons=await _lessonRepository.GetLessonByCourse(courseId);

        return lessons.Select(lesson=>new ViewLessonDdto
        {
            Id=lesson.Id,
            CourseId=lesson.CourseId,
            Title=lesson.Title,
        }).ToList();
    }

}