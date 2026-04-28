using FinancialDataTracker_Internship_.DTOs;
using FinancialDataTracker_Internship_.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinancialDataTracker_Internship_.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IStockService _stockService;

    public StocksController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StockDto>>> GetAll()
    {
        var stocks = await _stockService.GetAllAsync();

        return Ok(stocks);
    }

    [HttpGet("{symbol}")]
    public async Task<ActionResult<StockDto>> GetBySymbol(string symbol)
    {
        var stock = await _stockService.GetBySymbolAsync(symbol);

        if (stock is null)
        {
            return NotFound($"Stock with symbol '{symbol}' was not found.");
        }

        return Ok(stock);
    }

    [HttpPost]
    public async Task<ActionResult<StockDto>> Create(CreateStockRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Symbol))
        {
            return BadRequest("Stock symbol is required.");
        }

        var createdStock = await _stockService.CreateAsync(request);

        if (createdStock is null)
        {
            return BadRequest("Stock could not be created. It may already exist or the symbol may be invalid.");
        }

        return CreatedAtAction(
            nameof(GetBySymbol),
            new { symbol = createdStock.Symbol },
            createdStock);
    }

    [HttpPut("{symbol}/refresh")]
    public async Task<ActionResult<StockDto>> Refresh(string symbol)
    {
        var updatedStock = await _stockService.RefreshAsync(symbol);

        if (updatedStock is null)
        {
            return NotFound($"Stock with symbol '{symbol}' was not found or could not be refreshed.");
        }

        return Ok(updatedStock);
    }

    [HttpDelete("{symbol}")]
    public async Task<IActionResult> Delete(string symbol)
    {
        var deleted = await _stockService.DeleteAsync(symbol);

        if (!deleted)
        {
            return NotFound($"Stock with symbol '{symbol}' was not found.");
        }

        return NoContent();
    }
}