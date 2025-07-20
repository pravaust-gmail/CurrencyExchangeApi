using CurrencyExchangeApi.Common;
using CurrencyExchangeApi.Infrastructure;
using CurrencyExchangeApi.Models.Requests;
using CurrencyExchangeApi.Models.Responses;

namespace CurrencyExchangeApi.Services;

public interface IQuoteService
{
    Task<Result<CreateQuoteResponse>> CreateQuote(CreateQuoteRequest request);
    Result<CreateQuoteResponse> GetQuoteById(Guid quoteId);
}

public class QuoteService(IExchangeRateProvider exchangeRateProvider, IExchangeRateStore exchangeRateStore, IStore<CreateQuoteResponse> quoteStore) : IQuoteService
{
    public async Task<Result<CreateQuoteResponse>> CreateQuote(CreateQuoteRequest request)
    {
        var sellCurrency = request.SellCurrency;
        var buyCurrency = request.BuyCurrency;

        var cachedRate = exchangeRateStore.GetRate(sellCurrency, buyCurrency);
        
        var exchangeRate = cachedRate ?? await GetRateAndCacheAsync(sellCurrency, buyCurrency);

        var quote = MapToCreateQuoteResponse(request.Amount, exchangeRate);

        quoteStore.Save(quote);

        return Result<CreateQuoteResponse>.Ok(quote);
    }
    
    public Result<CreateQuoteResponse> GetQuoteById(Guid quoteId)
    {
        var quote = quoteStore.Get(quoteId);

        return quote is null
            ? Result<CreateQuoteResponse>.Fail($"Quote with ID '{quoteId}' was not found.")
            : Result<CreateQuoteResponse>.Ok(quote);
    }
    
    private async Task<decimal> GetRateAndCacheAsync(string sellCurrency, string buyCurrency)
    {
        var rate = await exchangeRateProvider.GetExchangeRate(sellCurrency, buyCurrency);
        exchangeRateStore.SaveRate(sellCurrency, buyCurrency, rate);
        return rate;
    }
    
    private static CreateQuoteResponse MapToCreateQuoteResponse(decimal amount, decimal exchangeRate)
    {
        return new CreateQuoteResponse
        {
            QuoteId = Guid.NewGuid(),
            OfxRate = exchangeRate,
            InverseOfxRate = Math.Round(1 / exchangeRate, 5),
            ConvertedAmount = Math.Round(amount * exchangeRate, 2)
        };
    }
}
