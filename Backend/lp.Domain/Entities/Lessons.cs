namespace lp.Domain.Entities;

public class Lesson
{
    public int Id{get;set;}
    public int CourseId{get;set;}
    public Course? Course{get;set;}
    public string Title{get;set;}=string.Empty;
    public string VideoUrl{get;set;}=string.Empty;
}