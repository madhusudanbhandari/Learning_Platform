using lp.Domain.Entities;
using lp.Application.Interface;
using lp.Application.Dtos;


namespace lp.Application.Interface;

public interface IPaymenetService
{
    Task<InitiatePaymentResponseDto>InitiatePayment(int studentId, int enrollId,string IdemKey);
    Task<ViewPaymentDto> VerifyKhaltiPaymentAsync(string pidx);
}