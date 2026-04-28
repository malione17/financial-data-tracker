using FinancialDataTracker_Internship_.DTOs;

namespace FinancialDataTracker_Internship_.Services;

public interface IStockService
{
    Task<List<StockDto>> GetAllAsync();

    Task<StockDto?> GetBySymbolAsync(string symbol);

    Task<StockDto?> CreateAsync(CreateStockRequest request);

    Task<StockDto?> RefreshAsync(string symbol);

    Task<bool> DeleteAsync(string symbol);
}