using FinancialDataTracker_Internship_.DTOs;
using FinancialDataTracker_Internship_.Models;
using FinancialDataTracker_Internship_.Repositories;

namespace FinancialDataTracker_Internship_.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;
    private readonly IFinancialApiClient _financialApiClient;

    public StockService(IStockRepository stockRepository, IFinancialApiClient financialApiClient)
    {
        _stockRepository = stockRepository;
        _financialApiClient = financialApiClient;
    }

    public async Task<List<StockDto>> GetAllAsync()
    {
        var stocks = await _stockRepository.GetAllAsync();

        return stocks.Select(MapToDto).ToList();
    }

    public async Task<StockDto?> GetBySymbolAsync(string symbol)
    {
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock is null)
        {
            return null;
        }

        return MapToDto(stock);
    }

    public async Task<StockDto?> CreateAsync(CreateStockRequest request)
    {
        var normalizedSymbol = request.Symbol.Trim().ToUpperInvariant();

        if (string.IsNullOrWhiteSpace(normalizedSymbol))
        {
            return null;
        }

        var exists = await _stockRepository.ExistsAsync(normalizedSymbol);

        if (exists)
        {
            return null;
        }

        var quote = await _financialApiClient.GetQuoteAsync(normalizedSymbol);

        if (quote is null)
        {
            return null;
        }

        var stock = new Stock
        {
            Symbol = normalizedSymbol,
            Name = string.IsNullOrWhiteSpace(request.Name) ? normalizedSymbol : request.Name.Trim(),
            CurrentPrice = quote.CurrentPrice,
            PreviousClosePrice = quote.PreviousClosePrice,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        await _stockRepository.AddAsync(stock);
        await _stockRepository.SaveChangesAsync();

        return MapToDto(stock);
    }

    public async Task<StockDto?> RefreshAsync(string symbol)
    {
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock is null)
        {
            return null;
        }

        var quote = await _financialApiClient.GetQuoteAsync(stock.Symbol);

        if (quote is null)
        {
            return null;
        }

        stock.CurrentPrice = quote.CurrentPrice;
        stock.PreviousClosePrice = quote.PreviousClosePrice;
        stock.LastUpdatedAt = DateTime.UtcNow;

        await _stockRepository.SaveChangesAsync();

        return MapToDto(stock);
    }

    public async Task<bool> DeleteAsync(string symbol)
    {
        var stock = await _stockRepository.GetBySymbolAsync(symbol);

        if (stock is null)
        {
            return false;
        }

        _stockRepository.Delete(stock);
        await _stockRepository.SaveChangesAsync();

        return true;
    }

    private static StockDto MapToDto(Stock stock)
    {
        return new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            Name = stock.Name,
            CurrentPrice = stock.CurrentPrice,
            PreviousClosePrice = stock.PreviousClosePrice,
            GrowthPercentage = CalculateGrowthPercentage(stock.CurrentPrice, stock.PreviousClosePrice),
            LastUpdatedAt = stock.LastUpdatedAt
        };
    }

    private static decimal CalculateGrowthPercentage(decimal currentPrice, decimal previousClosePrice)
    {
        if (previousClosePrice <= 0)
        {
            return 0;
        }

        return Math.Round((currentPrice - previousClosePrice) / previousClosePrice * 100, 2);
    }
}