using FinancialDataTracker_Internship_.DTOs;

namespace FinancialDataTracker_Internship_.Services;

public class FinnhubApiClient : IFinancialApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public FinnhubApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<FinnhubQuoteResponse?> GetQuoteAsync(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return null;
        }

        var apiKey = _configuration["Finnhub:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Finnhub API key is not configured.");
        }

        var normalizedSymbol = symbol.Trim().ToUpperInvariant();

        var response = await _httpClient.GetAsync(
            $"quote?symbol={Uri.EscapeDataString(normalizedSymbol)}&token={Uri.EscapeDataString(apiKey)}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var contentType = response.Content.Headers.ContentType?.MediaType;

        if (contentType is null || !contentType.Contains("json"))
        {
            var content = await response.Content.ReadAsStringAsync();

            throw new InvalidOperationException(
                $"Finnhub returned non-JSON response. Content-Type: {contentType}. Response starts with: {content[..Math.Min(content.Length, 100)]}");
        }

        var quote = await response.Content.ReadFromJsonAsync<FinnhubQuoteResponse>();

        if (quote is null || quote.CurrentPrice <= 0)
        {
            return null;
        }

        return quote;
    }
}