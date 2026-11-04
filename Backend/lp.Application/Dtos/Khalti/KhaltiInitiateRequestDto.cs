using System.Text.Json.Serialization;

namespace lp.Application.Dtos;

public class KhaltiInitiateRequestDto
{
    [JsonPropertyName("return_url")]
    public string ReturnUrl { get; set; } = string.Empty;
    [JsonPropertyName("website_url")]
    public string WebsiteUrl { get; set; } = string.Empty;
    [JsonPropertyName("amount")]
    public long Amount { get; set; }
    [JsonPropertyName("purchase_order_id")]
    public string PurchaseOrderId { get; set; } = string.Empty;
    [JsonPropertyName("purchase_order_name")]
    public string PurchaseOrderName { get; set; } = string.Empty;
}