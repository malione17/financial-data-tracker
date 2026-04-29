using FinancialDataTracker_Internship_.DTOs;
using FinancialDataTracker_Internship_.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinancialDataTracker_Internship_.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IStockRepository _stockRepository;

    public AnalyticsController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpGet("top-gainers")]
    public async Task<ActionResult<List<TopGainerDto>>> GetTopGainers([FromQuery] int count = 5)
    {
        if (count <= 0)
        {
            return BadRequest("Count must be greater than zero.");
        }

        var stocks = await _stockRepository.GetAllAsync();

        var topGainers = stocks
            .Where(stock => stock.PreviousClosePrice > 0)
            .Select(stock => new TopGainerDto
            {
                Symbol = stock.Symbol,
                Name = stock.Name,
                CurrentPrice = stock.CurrentPrice,
                PreviousClosePrice = stock.PreviousClosePrice,
                GrowthPercentage = Math.Round(
                    (stock.CurrentPrice - stock.PreviousClosePrice) / stock.PreviousClosePrice * 100,
                    2)
            })
            .OrderByDescending(stock => stock.GrowthPercentage)
            .Take(count)
            .ToList();

        return Ok(topGainers);
    }
}