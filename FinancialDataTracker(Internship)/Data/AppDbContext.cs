using FinancialDataTracker_Internship_.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialDataTracker_Internship_.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Stock> Stocks => Set<Stock>();
}