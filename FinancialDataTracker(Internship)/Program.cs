using FinancialDataTracker_Internship_.Data;
using Microsoft.EntityFrameworkCore;
using FinancialDataTracker_Internship_.Repositories;
using FinancialDataTracker_Internship_.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add services to the container.

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddHttpClient<IFinancialApiClient, FinnhubApiClient>(client =>
{
    var baseUrl = builder.Configuration["Finnhub:BaseUrl"];

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException("Finnhub BaseUrl is not configured.");
    }

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
