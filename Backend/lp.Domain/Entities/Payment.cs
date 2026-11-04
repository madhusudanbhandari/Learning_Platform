using lp.Domain.Entities.Enum;

namespace lp.Domain.Entities;


public class Payment
{
    public int Id{get;set;}
    public int EnrollId{get;set;}
    public Enrollment Enrollment{get;set;}=null!;
    public decimal Amount{get;set;}
    public string? TransactionId{get;set;}
    public DateTime CreatedAt{get;set;}
    public DateTime? PaidAt{get;set;}
    public PaymentStatus Status{get;set;}
}