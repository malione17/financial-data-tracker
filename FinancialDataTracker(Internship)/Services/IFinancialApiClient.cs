using FinancialDataTracker_Internship_.DTOs;

namespace FinancialDataTracker_Internship_.Services;

public interface IFinancialApiClient
{
    Task<FinnhubQuoteResponse?> GetQuoteAsync(string symbol);
}