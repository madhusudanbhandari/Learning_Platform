using lp.Domain.Entities;

namespace lp.Application.Interface;

public interface IPaymentRepository
{
    Task<Payment?> ViewPayment(int id);
    Task<Payment?> GetLatestPaymentByEnrollId(int enrollId);
    Task AddPayment(Payment payment);
    Task SaveChangesAsync();
}