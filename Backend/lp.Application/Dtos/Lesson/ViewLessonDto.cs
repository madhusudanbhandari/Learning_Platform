namespace lp.Application.Dtos;

public class ViewLessonDdto
{
   
    public int Id{get;set;}
    public int CourseId{get;set;}
    public string Title{get;set;}=string.Empty;
    public string VideoUrl{get;set;}=string.Empty;
}