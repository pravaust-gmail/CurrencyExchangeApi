# CurrencyExchangeAPI

CurrencyExchangeAPI is a .NET Web API that enables users to create currency exchange quotes, retrieve quotes, and initiate transfers based on those quotes.

---

## Prerequisites

- [.NET SDK 8.0 or later](https://dotnet.microsoft.com/download)
- Optional: IDE such as [Visual Studio](https://visualstudio.microsoft.com/), [JetBrains Rider](https://www.jetbrains.com/rider/), or [VS Code](https://code.visualstudio.com/)

---

## Building and Running the API

Navigate to the root folder containing the solution file (`CurrencyExchangeAPI.sln`) 

To build the solution:

```bash
dotnet build
```

Navigate to the API project folder and run:
```bash
cd CurrencyExchangeAPI
dotnet run
```

This starts the Web API locally (usually at https://localhost:5001 or http://localhost:5000).

Then navigate to http://localhost:5001/swagger/index.html (or the relevant port number) to access Swagger UI.

## Running Tests

Navigate to the root folder containing the solution file (`CurrencyExchangeAPI.sln`)

To run tests:
```bash
dotnet test
```