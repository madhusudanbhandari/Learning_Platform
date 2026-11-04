namespace lp.Application.Dtos;

public class InitiatePaymentResponseDto
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Pidx { get; set; } = string.Empty;
    public string PaymentUrl { get; set; } = string.Empty;
}