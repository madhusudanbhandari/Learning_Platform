using System.Text.Json.Serialization;

namespace lp.Application.Dtos;

public class KhaltiLookupResponseDto
{
    [JsonPropertyName("pidx")]
    public string Pidx { get; set; } = string.Empty;

    [JsonPropertyName("total_amount")]
    public long TotalAmount { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("fee")]
    public long Fee { get; set; }

    [JsonPropertyName("refunded")]
    public bool Refunded { get; set; }
}