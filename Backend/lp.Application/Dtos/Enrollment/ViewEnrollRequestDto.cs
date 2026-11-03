using lp.Domain.Entities.Enum;

namespace lp.Application.Dtos;

public class ViewEnrollRequestDto
{
    public int Id{get;set;}
    public int StudentId{get;set;}
    public int CourseId{get;set;}
    public EnrollmentStatus Status{get;set;}
    public decimal AmountPaid{get;set;}
    public DateTime CreatedAt{get;set;}
    public string Remark{get;set;}=string.Empty;
}