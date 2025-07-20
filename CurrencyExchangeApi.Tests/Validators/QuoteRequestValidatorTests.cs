using CurrencyExchangeApi.Validators;
using CurrencyExchangeApi.Models.Requests;

namespace CurrencyExchangeApi.Tests.Validators;

public class QuoteRequestValidatorTests
{
    private readonly QuoteRequestValidator _validator = new();

    [Theory]
    [MemberData(nameof(GetQuoteRequestTestData))]
    public void Validate_ReturnsExpectedErrors(CreateQuoteRequest request, List<string> expectedErrors)
    {
        // Act
        var errors = _validator.Validate(request).ToList();

        // Assert
        Assert.Equal(expectedErrors.OrderBy(e => e), errors.OrderBy(e => e));
    }

    public static IEnumerable<object[]> GetQuoteRequestTestData()
    {
        yield return new object[]
        {
            new CreateQuoteRequest { SellCurrency = "AUD", BuyCurrency = "USD", Amount = 100 },
            new List<string>()
        };

        yield return new object[]
        {
            new CreateQuoteRequest { SellCurrency = "AUD", BuyCurrency = "XYZ", Amount = 100 },
            new List<string> { "XYZ' is not a supported BuyCurrency." }
        };

        yield return new object[]
        {
            new CreateQuoteRequest { SellCurrency = "XYZ", BuyCurrency = "USD", Amount = 100 },
            new List<string> { "XYZ' is not a supported SellCurrency." }
        };

        yield return new object[]
        {
            new CreateQuoteRequest { SellCurrency = "XYZ", BuyCurrency = "ABC", Amount = 100 },
            new List<string>
            {
                "XYZ' is not a supported SellCurrency.",
                "ABC' is not a supported BuyCurrency."
            }
        };

        yield return new object[]
        {
            new CreateQuoteRequest { SellCurrency = "USD", BuyCurrency = "USD", Amount = 100 },
            new List<string> { "SellCurrency and BuyCurrency cannot be the same." }
        };

        yield return new object[]
        {
            new CreateQuoteRequest { SellCurrency = "XYZ", BuyCurrency = "XYZ", Amount = 100 },
            new List<string>
            {
                "XYZ' is not a supported SellCurrency.",
                "XYZ' is not a supported BuyCurrency.",
                "SellCurrency and BuyCurrency cannot be the same."
            }
        };
    }
}
