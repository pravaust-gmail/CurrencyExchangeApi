using CurrencyExchangeApi.Models.Requests;

namespace CurrencyExchangeApi.Validators;

public interface IQuoteRequestValidator
{
    IEnumerable<string> Validate(CreateQuoteRequest request);
}

public class QuoteRequestValidator : IQuoteRequestValidator
{
    private static readonly HashSet<string> SupportedSellCurrencies = new() { "AUD", "USD", "EUR" };
    private static readonly HashSet<string> SupportedBuyCurrencies = new() { "USD", "INR", "PHP" };

    public IEnumerable<string> Validate(CreateQuoteRequest request)
    {
        if (!SupportedSellCurrencies.Contains(request.SellCurrency))
            yield return $"{request.SellCurrency}' is not a supported SellCurrency.";

        if (!SupportedBuyCurrencies.Contains(request.BuyCurrency))
            yield return $"{request.BuyCurrency}' is not a supported BuyCurrency.";

        if (request.SellCurrency == request.BuyCurrency)
            yield return "SellCurrency and BuyCurrency cannot be the same.";
    }
}