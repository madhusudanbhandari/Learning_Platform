using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IPaymentRepository
{
    Task<Payment?> ViewPayment(int id);
    Task<Payment?> GetPaymentByIdempotency(string key); 
    Task<Payment?> GetLatestPaymentByEnrollId(int enrollId);
    Task AddPayment(Payment payment);
    Task SaveChangesAsync();

    Task<Payment?> GetPaymentByPidx(string pidx);
}