using System.Security.Claims;
using lp.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lp.Api.Controller;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymenetService _paymentService;

    public PaymentController(IPaymenetService paymenetService)
    {
        _paymentService=paymenetService;
    }

    [Authorize(Roles ="Student")]
    [HttpPost("Initiate")]
    public async Task<IActionResult> InitiatePayment(int enrollId,
                                        [FromHeader(Name = "Idempotency-Key")]string idempotencyKey)

    {
        var student=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(student,out int studentId))
        {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return BadRequest("Idempotency key is required");
        }

        var payment=await _paymentService.InitiatePayment(studentId,enrollId,idempotencyKey);

        return Ok(payment);
    }

    // [HttpPost("Confirm")]
    // public async Task<IActionResult> ConfirmPayment(int paymentId, bool IsSuccessfull, string? transactionId)
    // {
    //     var confirm=await _paymenetService.ConfirmPayment(paymentId,IsSuccessfull,transactionId);

    //     return Ok(confirm);
    // }

    [AllowAnonymous]
    [HttpGet("khalti/callback")]
    public async Task<IActionResult> KhaltiCallback(
        [FromQuery] string pidx)
    {
        if (string.IsNullOrWhiteSpace(pidx))
        {
            return BadRequest("Missing Khalti payment reference");
        }

        var result = await _paymentService.VerifyKhaltiPaymentAsync(pidx);

        return Ok(result);
    }
}