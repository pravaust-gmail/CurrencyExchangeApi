using System.Collections.Concurrent;

namespace CurrencyExchangeApi.Infrastructure;

public interface IExchangeRateStore
{
    void SaveRate(string sellCurrency, string buyCurrency, decimal rate);
    decimal? GetRate(string sellCurrency, string buyCurrency);
}

public class InMemoryExchangeRateStore : IExchangeRateStore
{
    private readonly ConcurrentDictionary<(string, string), decimal> _rates = new();

    public void SaveRate(string sellCurrency, string buyCurrency, decimal rate)
    {
        _rates[(sellCurrency.ToUpperInvariant(), buyCurrency.ToUpperInvariant())] = rate;
    }

    public decimal? GetRate(string sellCurrency, string buyCurrency)
    {
        if (_rates.TryGetValue((sellCurrency.ToUpperInvariant(), buyCurrency.ToUpperInvariant()), out var rate))
            return rate;

        return null;
    }
}