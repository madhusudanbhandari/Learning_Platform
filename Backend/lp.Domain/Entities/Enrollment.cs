using lp.Domain.Entities.Enum;

namespace lp.Domain.Entities;

public class Enrollment
{
    public int Id{get;set;}
    public int StudentId{get;set;}
    public User Student{get;set;}=null!;
    public int CourseId{get;set;}
    public Course Course{get;set;}=null!;
    public EnrollmentStatus Status{get;set;}
    public decimal AmountPaid{get;set;}
    public DateTime CreatedAt{get;set;}
    public string Remark{get;set;}=string.Empty;
}