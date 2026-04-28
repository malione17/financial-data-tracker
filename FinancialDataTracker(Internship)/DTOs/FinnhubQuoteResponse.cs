using System.Text.Json.Serialization;

namespace FinancialDataTracker_Internship_.DTOs;

public class FinnhubQuoteResponse
{
    [JsonPropertyName("c")]
    public decimal CurrentPrice { get; set; }

    [JsonPropertyName("pc")]
    public decimal PreviousClosePrice { get; set; }
}