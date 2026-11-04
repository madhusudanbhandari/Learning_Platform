using System.Text.Json;
using lp.Application.Interface;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using lp.Application.Dtos;
using System.Net.Http.Json;
using lp.Application.Options;

namespace lp.Infrastructure.PaymentGateway;

public class KhaltiPaymentGateway : IPaymentGateway
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy=JsonNamingPolicy.SnakeCaseLower
    };

    public KhaltiPaymentGateway(HttpClient httpClient, IOptions<KhaltiOptions> options)
    {
        _httpClient=httpClient;
        
        _httpClient.BaseAddress=new Uri(options.Value.BaseUrl);

        _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Key",options.Value.SecretKey);
    }

    public async Task<KhaltiInitiateResponseDto> InitiatePayment(
                                    KhaltiInitiateRequestDto request,
                                    CancellationToken cancellationToken = default)
    {
        var response=await _httpClient.PostAsJsonAsync("epayment/initiate",
                                                        request,
                                                        _serializerOptions,
                                                        cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error=await response.Content.ReadAsStringAsync(cancellationToken);

            throw new HttpRequestException($" Khalti payment initiation failed:{error}");

            
        }

        var result=await response.Content.ReadFromJsonAsync<KhaltiInitiateResponseDto>(
            _serializerOptions,
            cancellationToken
        );
        return result ?? throw new InvalidOperationException("Khalti returned as empty response");
    }

    public async Task<KhaltiLookupResponseDto> LookupPaymentAsync(
        string pidx, CancellationToken cancellationToken=default
    )
    {
        var request=new KhaltiLookupRequestDto
        {
            Pidx=pidx
        };

        var response=await _httpClient.PostAsJsonAsync("epayment/lookup/",request,cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
                    var error = await response.Content.ReadAsStringAsync(
            cancellationToken);

        throw new HttpRequestException(
            $"Khalti payment lookup failed: {error}");
         }
        var result =
                await response.Content.ReadFromJsonAsync<KhaltiLookupResponseDto>(
                    cancellationToken: cancellationToken);

            return result ?? throw new InvalidOperationException(
                "Khalti returned an empty lookup response");
        }


}