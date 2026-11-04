using AutoMapper;
using lp.Application.Dtos;
using lp.Application.Exceptions;
using lp.Application.Interface;
using lp.Application.Options;
using lp.Domain.Entities;
using Microsoft.Extensions.Options;
using lp.Domain.Entities.Enum;

namespace lp.Application.Service;
public class PaymentService : IPaymenetService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IPaymentGateway _paymentGateway;
    private readonly KhaltiOptions _khaltiOptions;
    public PaymentService(
                    IPaymentRepository paymentRepository,
                    IEnrollmentRepository enrollmentRepository,
                    IPaymentGateway paymentGateway,
                    IOptions<KhaltiOptions> options)
    {
        _paymentRepository=paymentRepository;
        _enrollmentRepository=enrollmentRepository;
        _paymentGateway=paymentGateway;
        _khaltiOptions=options.Value;
    }

    public async Task<InitiatePaymentResponseDto> InitiatePayment(int studentId, int enrollId)
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
            CreatedAt=DateTime.UtcNow,
            Status=Domain.Entities.Enum.PaymentStatus.Pending           

        };

        await _paymentRepository.AddPayment(payment);
        await _paymentRepository.SaveChangesAsync();

        try{
            var amount=(long) enrollment.Course.Price;

            var khaltiRequest=new KhaltiInitiateRequestDto
            {
                ReturnUrl=_khaltiOptions.ReturnUrl,
                WebsiteUrl=_khaltiOptions.WebsiteUrl,
                Amount=amount,
                PurchaseOrderId=payment.Id.ToString(),
                PurchaseOrderName=enrollment.Course.Title
            };

            var khaltiResponse=await _paymentGateway.InitiatePayment(khaltiRequest);

            if(string.IsNullOrWhiteSpace(khaltiResponse.Pidx) ||
                string.IsNullOrWhiteSpace(khaltiResponse.PaymentUrl))
            {
                throw new InvalidOperationException("Invalid response from khalti");
            }

            payment.Pidx=khaltiResponse.Pidx;
            await _paymentRepository.SaveChangesAsync();

            return new InitiatePaymentResponseDto
            {
                PaymentId = payment.Id,
                Amount = payment.Amount,
                Pidx = payment.Pidx,
                PaymentUrl = khaltiResponse.PaymentUrl
            };
        }
        catch
        {
            payment.Status=Domain.Entities.Enum.PaymentStatus.Failed;

            await _paymentRepository.SaveChangesAsync();

            throw;
        }

        
    }



    // public async Task<ViewPaymentDto> ConfirmPayment(int paymentId, bool IsSuccessfull, string? transactionId)
    // {
    //     var payment=await _paymentRepository.ViewPayment(paymentId);

    //     if (payment == null)
    //     {
    //         throw new NotFoundException("Cannot find the payment");
    //     }

    //     if (payment.Status != Domain.Entities.Enum.PaymentStatus.Pending)
    //     {
    //         throw new InvalidOperationException("Payment is already processed");
    //     }

    //     if (IsSuccessfull)
    //     {
    //         payment.Status=Domain.Entities.Enum.PaymentStatus.Completed;
    //         payment.PaidAt=DateTime.UtcNow;
    //         payment.TransactionId=transactionId;

    //         payment.Enrollment.AmountPaid+=payment.Amount;
    //     }
    //     else
    //     {
    //         payment.Status=Domain.Entities.Enum.PaymentStatus.Failed;
    //     }

    //     await _paymentRepository.SaveChangesAsync();

    //     return new ViewPaymentDto
    //     {
    //         Id=payment.Id,
    //         EnrollId=payment.EnrollId,
    //         Amount=payment.Amount,
    //         CreatedAt=payment.CreatedAt,
    //         TransactionId=payment.TransactionId,
    //         Status=payment.Status,
    //         PaidAt=payment.PaidAt
            
    //     };        

    // }

    public async Task<ViewPaymentDto> VerifyKhaltiPaymentAsync(string pidx)
{
    var payment = await _paymentRepository.GetPaymentByPidx(pidx);

    if (payment == null)
    {
        throw new NotFoundException("Payment not found");
    }

    if (payment.Status == PaymentStatus.Completed)
    {
        return MapPayment(payment);
    }

    if (payment.Status != PaymentStatus.Pending)
    {
        throw new InvalidOperationException(
            "This payment is no longer pending");
    }

    var result = await _paymentGateway.LookupPaymentAsync(pidx);

    if (!string.Equals(result.Pidx, payment.Pidx, StringComparison.Ordinal))
    {
        throw new InvalidOperationException(
            "Khalti payment reference mismatch");
    }

    var expectedAmount = checked((long)(payment.Amount * 100m));

    if (result.TotalAmount != expectedAmount)
    {
        throw new InvalidOperationException(
            "Payment amount mismatch");
    }

    if (result.Status.Equals(
        "Completed",
        StringComparison.OrdinalIgnoreCase))
    {
        if (result.Refunded ||
            string.IsNullOrWhiteSpace(result.TransactionId))
        {
            throw new InvalidOperationException(
                "Invalid completed payment response");
        }

        payment.Status = PaymentStatus.Completed;
        payment.TransactionId = result.TransactionId;
        payment.PaidAt = DateTime.UtcNow;

        payment.Enrollment.Status = EnrollmentStatus.Pending;

        payment.Enrollment.AmountPaid = payment.Amount;
    }
    else if (
        result.Status.Equals("Expired", StringComparison.OrdinalIgnoreCase) ||
        result.Status.Equals("User canceled", StringComparison.OrdinalIgnoreCase) ||
        result.Status.Equals("Failed", StringComparison.OrdinalIgnoreCase))
    {
        payment.Status = PaymentStatus.Failed;
    }
    else if (
        result.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase) ||
        result.Status.Equals("Initiated", StringComparison.OrdinalIgnoreCase))
    {
    }
    else
    {
        throw new InvalidOperationException(
            $"Payment requires further investigation. Khalti status: {result.Status}");
    }

    await _paymentRepository.SaveChangesAsync();

    return MapPayment(payment);
}
private static ViewPaymentDto MapPayment(Payment payment)
{
    return new ViewPaymentDto
    {
        Id = payment.Id,
        EnrollId = payment.EnrollId,
        Amount = payment.Amount,
        CreatedAt = payment.CreatedAt,
        TransactionId = payment.TransactionId,
        Status = payment.Status,
        PaidAt = payment.PaidAt
    };
}
}