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
        var apiKey = _configuration["Finnhub:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Finnhub API key is not configured.");
        }

        var normalizedSymbol = symbol.ToUpperInvariant();

        var response = await _httpClient.GetAsync($"/quote?symbol={normalizedSymbol}&token={apiKey}");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var quote = await response.Content.ReadFromJsonAsync<FinnhubQuoteResponse>();

        if (quote is null || quote.CurrentPrice <= 0)
        {
            return null;
        }

        return quote;
    }
}