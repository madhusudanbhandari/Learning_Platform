using lp.Application.Interface;
using lp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace lp.Infrastructure.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly LpDbConext _context;
    public PaymentRepository(LpDbConext conext)
    {
        _context=conext;
    }

    public async Task<Payment?> ViewPayment(int id)
    {
        return await _context.Payments.FirstOrDefaultAsync(p=>p.Id==id);
    }

    public async Task<Payment?> GetLatestPaymentByEnrollId(int enrollId)
    {
        return await _context.Payments
                            .Where(p=>p.EnrollId==enrollId)
                            .OrderByDescending(p=>p.CreatedAt)
                            .FirstOrDefaultAsync();
    }

    public async Task AddPayment(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
    }

    public async Task SaveChangesAsync()
    {
         await _context.SaveChangesAsync();
    }
}