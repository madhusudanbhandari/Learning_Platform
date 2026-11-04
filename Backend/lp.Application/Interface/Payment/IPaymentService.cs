using lp.Domain.Entities;
using lp.Application.Interface;
using lp.Application.Dtos;


namespace lp.Application.Interface;

public interface IPaymenetService
{
    Task<ViewPaymentDto>InitiatePayment(int studentId, int enrollId);
    Task<Payment> ConfirmPayment(int paymentId, bool succcessfull, string? transactionId);
}