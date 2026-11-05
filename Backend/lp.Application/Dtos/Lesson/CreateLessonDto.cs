namespace lp.Application.Dtos;

public class CreateLesson
{
    public int CourseId{get;set;}
    public string Title{get;set;}=string.Empty;
    public string VideoUrl{get;set;}=string.Empty;

}