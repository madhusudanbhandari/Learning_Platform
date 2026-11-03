namespace lp.Application.Dtos;

public class CreateCourseDto
{
    public string Title{get;set;}=string.Empty;
    public string Field{get;set;}=string.Empty;
    public decimal Price{get;set;}
    public string Duration{get;set;}=string.Empty;
}