namespace lp.Domain.Entities;


public class Payment
{
    public int Id{get;set;}
    public int CourseId{get;set;}
    public Course Course{get;set;}=null!;
    public int StudentId{get;set;}
    public User User{get;set;}=null!;
    public decimal Amount{get;set;}
    public DateTime PaidAt{get;set;}
}