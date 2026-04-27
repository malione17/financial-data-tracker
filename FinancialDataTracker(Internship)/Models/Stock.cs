namespace FinancialDataTracker_Internship_.Models;

public class Stock
{
    public int Id { get; set; }

    public string Symbol { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal CurrentPrice { get; set; }

    public decimal PreviousClosePrice { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}