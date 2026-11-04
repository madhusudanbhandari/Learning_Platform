using lp.Domain.Entities.Enum;

namespace lp.Application.Dtos;

public class ViewPaymentDto
{
    public int Id{get;set;}
    public int EnrollId{get;set;}
    public decimal Amount{get;set;}
    public string? TransactionId{get;set;}
    public DateTime CreatedAt{get;set;}
    public DateTime? PaidAt{get;set;}
    public PaymentStatus Status{get;set;}

}