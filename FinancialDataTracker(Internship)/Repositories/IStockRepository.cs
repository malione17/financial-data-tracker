using FinancialDataTracker_Internship_.Models;

namespace FinancialDataTracker_Internship_.Repositories;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();

    Task<Stock?> GetBySymbolAsync(string symbol);

    Task<bool> ExistsAsync(string symbol);

    Task AddAsync(Stock stock);

    void Delete(Stock stock);

    Task SaveChangesAsync();
}