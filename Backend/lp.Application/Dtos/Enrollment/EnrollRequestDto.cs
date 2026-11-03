namespace lp.Application.Dtos;

public class EnrollRequestDto
{
    public int Id{get;set;}
    public int StudentId{get;set;}
    public int CourseId{get;set;}
    public decimal AmountPaid{get;set;}
    public DateTime CreatedAt{get;set;}
}