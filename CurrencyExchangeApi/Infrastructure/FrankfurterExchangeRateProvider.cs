using System.Text.Json;
using CurrencyExchangeApi.Models.External;

namespace CurrencyExchangeApi.Infrastructure;

public interface IExchangeRateProvider
{
    Task<decimal> GetExchangeRate(string sellCurrency, string buyCurrency);
}

public class FrankfurterExchangeRateProvider(HttpClient httpClient) : IExchangeRateProvider
{
    public async Task<decimal> GetExchangeRate(string sellCurrency, string buyCurrency)
    {
        var url = $"https://api.frankfurter.app/latest?from={sellCurrency}&to={buyCurrency}";

        var result = await httpClient.GetFromJsonAsync<FrankfurterResponse>(url)
                     ?? throw new Exception("Null response from Frankfurter API.");

        if (!result.Rates.TryGetValue(buyCurrency, out var rate))
            throw new Exception($"Missing rate for {buyCurrency} in Frankfurter API response.");

        return rate;
    }
}