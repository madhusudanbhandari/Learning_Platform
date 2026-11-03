namespace lp.Domain.Entities;

public class Course
{
    public int Id{get;set;}
    public string Title{get;set;}=string.Empty;
    public string Field{get;set;}=string.Empty;
    public decimal Price{get;set;}
    public string Duration{get;set;}=string.Empty;

    public ICollection<Enrollment> Enrollments{get;set;}=new List<Enrollment>();


}