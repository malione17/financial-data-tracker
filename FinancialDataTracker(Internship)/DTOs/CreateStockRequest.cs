namespace FinancialDataTracker_Internship_.DTOs;

public class CreateStockRequest
{
    public string Symbol { get; set; } = string.Empty;

    public string? Name { get; set; }
}