using FinancialDataTracker_Internship_.Data;
using FinancialDataTracker_Internship_.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialDataTracker_Internship_.Repositories;

// Design Pattern: Repository Pattern
// Database access logic is isolated here so services do not depend directly on Entity Framework.
public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks
            .OrderBy(stock => stock.Symbol)
            .ToListAsync();
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        var normalizedSymbol = symbol.ToUpperInvariant();

        return await _context.Stocks
            .FirstOrDefaultAsync(stock => stock.Symbol == normalizedSymbol);
    }

    public async Task<bool> ExistsAsync(string symbol)
    {
        var normalizedSymbol = symbol.ToUpperInvariant();

        return await _context.Stocks
            .AnyAsync(stock => stock.Symbol == normalizedSymbol);
    }

    public async Task AddAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
    }

    public void Delete(Stock stock)
    {
        _context.Stocks.Remove(stock);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}