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
    private readonly IPaymenetService _paymenetService;

    public PaymentController(IPaymenetService paymenetService)
    {
        _paymenetService=paymenetService;
    }

    [Authorize(Roles ="Student")]
    [HttpPost("Initiate")]
    public async Task<IActionResult> InitiatePayment(int enrollId)
    {
        var student=User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(!int.TryParse(student,out int studentId))
        {
            return Unauthorized();
        }

        var payment=await _paymenetService.InitiatePayment(studentId,enrollId);

        return Ok(payment);
    }

    // [HttpPost("Confirm")]
    // public async Task<IActionResult> ConfirmPayment(int paymentId, bool IsSuccessfull, string? transactionId)
    // {
    //     var confirm=await _paymenetService.ConfirmPayment(paymentId,IsSuccessfull,transactionId);

    //     return Ok(confirm);
    // }
}