using System.Text.Json.Serialization;

namespace lp.Application.Dtos;

public class KhaltiInitiateResponseDto
{
    [JsonPropertyName("pidx")]
    public string Pidx { get; set; } = string.Empty;
    [JsonPropertyName("payment_url")]
    public string PaymentUrl { get; set; } = string.Empty;
    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}