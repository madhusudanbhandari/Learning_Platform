using System.Text.Json.Serialization;

namespace lp.Application.Dtos;

public class KhaltiLookupRequestDto
{
    [JsonPropertyName("pidx")]
    public string Pidx { get; set; } = string.Empty;
}