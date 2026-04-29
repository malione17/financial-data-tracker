# Financial Data Tracker

Financial Data Tracker is a .NET 8 Web API project developed for a software engineering internship technical assessment.

The project allows users to create and manage a stock watchlist. It retrieves financial price data from the Finnhub API, stores the data in a local SQLite database, and provides RESTful endpoints for CRUD and analytics operations.

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQLite
- Finnhub API
- Swagger / OpenAPI
- Repository Pattern

## Features

- Add a stock to the watchlist
- List all tracked stocks
- Get a stock by symbol
- Refresh stock price data from Finnhub
- Delete a stock from the watchlist
- View top gaining stocks based on growth percentage
- Store stock data in a local SQLite database
- Keep API keys secure using .NET user-secrets

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/stocks` | Returns all tracked stocks |
| GET | `/api/stocks/{symbol}` | Returns a stock by symbol |
| POST | `/api/stocks` | Adds a new stock to the watchlist |
| PUT | `/api/stocks/{symbol}/refresh` | Refreshes stock price data from Finnhub |
| DELETE | `/api/stocks/{symbol}` | Deletes a stock from the watchlist |
| GET | `/api/analytics/top-gainers?count=5` | Returns the top gaining stocks |

## Example Request

POST `/api/stocks`

```json
{
  "symbol": "AAPL",
  "name": "Apple Inc."
}
Example Response
{
  "id": 1,
  "symbol": "AAPL",
  "name": "Apple Inc.",
  "currentPrice": 270.71,
  "previousClosePrice": 267.61,
  "growthPercentage": 1.16,
  "lastUpdatedAt": "2026-04-29T13:00:13.2965767Z"
}
Analytics Logic

The top gainers endpoint calculates growth percentage using the following formula:

GrowthPercentage = (CurrentPrice - PreviousClosePrice) / PreviousClosePrice * 100

The result is ordered from the highest growth percentage to the lowest.

Design Pattern

This project uses the Repository Pattern.

The repository layer isolates database access logic from the service and controller layers. This keeps the business logic cleaner and prevents controllers from depending directly on Entity Framework Core.

The implementation can be found in:

Repositories/IStockRepository.cs
Repositories/StockRepository.cs
External API

This project uses the Finnhub API to retrieve financial quote data.

The Finnhub API key is not stored in appsettings.json and should not be committed to GitHub.

Configuration

The appsettings.json file contains the Finnhub base URL and an empty API key value:

"Finnhub": {
  "BaseUrl": "https://finnhub.io/api/v1/",
  "ApiKey": ""
}

To configure the Finnhub API key locally, use .NET user-secrets:

dotnet user-secrets init
dotnet user-secrets set "Finnhub:ApiKey" "YOUR_FINNHUB_API_KEY"

To verify the key:

dotnet user-secrets list
Database

The project uses SQLite as the local database.

The database connection string is configured in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Data Source=financialtracker.db"
}

Entity Framework Core migrations are included in the project.

To apply migrations:

Update-Database

or using the .NET CLI:

dotnet ef database update
How to Run
Clone the repository.
git clone https://github.com/malione17/financial-data-tracker.git
Open the project in Visual Studio.
Configure the Finnhub API key using user-secrets.
dotnet user-secrets set "Finnhub:ApiKey" "YOUR_FINNHUB_API_KEY"
Apply database migrations.
dotnet ef database update
Run the project.
Open Swagger in the browser.
https://localhost:7036/swagger

or:

http://localhost:5150/swagger
Notes
The real Finnhub API key should never be committed to GitHub.
Local SQLite database files are ignored by .gitignore.
Swagger is enabled for API testing.
The project is organized into Controllers, Services, Repositories, DTOs, Models, and Data layers