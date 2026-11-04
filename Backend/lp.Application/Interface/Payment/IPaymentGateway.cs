using lp.Application.Dtos;

namespace lp.Application.Interface;

public interface IPaymentGateway
{
    public Task<KhaltiInitiateResponseDto> InitiatePayment(
        KhaltiInitiateRequestDto dto,
        CancellationToken cancellationToken=default
    );

    Task<KhaltiLookupResponseDto> LookupPaymentAsync(
    string pidx,
    CancellationToken cancellationToken = default);
}