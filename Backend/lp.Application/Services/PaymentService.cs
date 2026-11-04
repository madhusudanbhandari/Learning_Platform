using AutoMapper;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Domain.Entities;

namespace lp.Application.Service;

public class PaymentService : IPaymenetService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    public PaymentService(
                    IPaymentRepository paymentRepository,
                    IEnrollmentRepository enrollmentRepository)
    {
        _paymentRepository=paymentRepository;
        _enrollmentRepository=enrollmentRepository;
    }

    public async Task<ViewPaymentDto> InitiatePayment(int studentId, int enrollId)
    {
        var enrollment=await _enrollmentRepository.GetEnrollmentRequestById(enrollId);

        if (enrollment == null)
        {
            throw new NotFoundException("Cannot find the enrollment");
        }

        if (enrollment.StudentId != studentId)
        {
            throw new UnauthorizedAccessException("You cannot initiate payment for others enrollments");
        }

        var existingPayment=await _paymentRepository.GetLatestPaymentByEnrollId(enrollment.Id);

        if (existingPayment != null)
        {
            if (existingPayment.Status == Domain.Entities.Enum.PaymentStatus.Pending)
            {
                throw new InvalidOperationException("A Payment for this enrollment is in pending state");
            }

            if (existingPayment.Status == Domain.Entities.Enum.PaymentStatus.Completed)
            {
                throw new InvalidOperationException("Payment already completed");
            }
        }

        var payment=new Payment
        {
            EnrollId=enrollment.Id,
            Amount=enrollment.Course.Price,
            TransactionId="",
            
            CreatedAt=DateTime.UtcNow,
            Status=Domain.Entities.Enum.PaymentStatus.Pending           

        };

        await _paymentRepository.AddPayment(payment);
        await _paymentRepository.SaveChangesAsync();

        return new ViewPaymentDto
        {
            Id=payment.Id,
            EnrollId=payment.EnrollId,
            Amount=payment.Amount,
            CreatedAt=payment.CreatedAt,
            TransactionId=payment.TransactionId,
            Status=payment.Status,
            PaidAt=payment.PaidAt
            
        };
    }

    public async Task<Payment> ConfirmPayment(int paymentId, bool IsSuccessfull, string? transactionId)
    {
        var payment=await _paymentRepository.ViewPayment(paymentId);

        if (payment == null)
        {
            throw new NotFoundException("Cannot find the payment");
        }

        if (payment.Status != Domain.Entities.Enum.PaymentStatus.Pending)
        {
            throw new InvalidOperationException("Payment is already processed");
        }

        if (IsSuccessfull)
        {
            payment.Status=Domain.Entities.Enum.PaymentStatus.Completed;
            payment.PaidAt=DateTime.UtcNow;
            payment.TransactionId=transactionId;

            payment.Enrollment.Status=Domain.Entities.Enum.EnrollmentStatus.Verified;
            payment.Enrollment.AmountPaid+=payment.Amount;
        }
        else
        {
            payment.Status=Domain.Entities.Enum.PaymentStatus.Failed;
        }

        await _paymentRepository.SaveChangesAsync();
        return payment;        

    }
}