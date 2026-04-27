namespace FinancialDataTracker_Internship_.DTOs;

public class TopGainerDto
{
    public string Symbol { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal CurrentPrice { get; set; }

    public decimal PreviousClosePrice { get; set; }

    public decimal GrowthPercentage { get; set; }
}