using CurrencyExchangeApi.Infrastructure;
using Moq;
using CurrencyExchangeApi.Models.Requests;
using CurrencyExchangeApi.Models.Responses;
using CurrencyExchangeApi.Services;

namespace CurrencyExchangeApi.Tests.Services;

public class QuoteServiceTests
{
    private readonly Mock<IStore<CreateQuoteResponse>> _mockQuoteStore;
    private readonly QuoteService _service;

    public QuoteServiceTests()
    {
        var mockRateProvider = new Mock<IExchangeRateProvider>();
        var mockRateStore = new Mock<IExchangeRateStore>();
        _mockQuoteStore = new Mock<IStore<CreateQuoteResponse>>();
        _service = new QuoteService(mockRateProvider.Object, mockRateStore.Object, _mockQuoteStore.Object);
    }
    
    [Fact]
    public async Task CreateQuote_SavesAndReturnsSuccess()
    {
        // Arrange
        var mockRate = 0.768333m;
        var mockExchangeRateProvider = new Mock<IExchangeRateProvider>();
        var mockQuoteStore = new Mock<IStore<CreateQuoteResponse>>();
        var mockExchangeRateStore = new Mock<IExchangeRateStore>();
        
        mockExchangeRateProvider
            .Setup(p => p.GetExchangeRate("AUD", "USD"))
            .ReturnsAsync(mockRate);

        var quoteService = new QuoteService(mockExchangeRateProvider.Object, mockExchangeRateStore.Object, mockQuoteStore.Object);

        var request = new CreateQuoteRequest
        {
            SellCurrency = "AUD",
            BuyCurrency = "USD",
            Amount = 100
        };
        
        var expectedInverseRate = Math.Round(1 / mockRate, 5);
        var expectedConvertedAmount = Math.Round(100 * mockRate, 2);

        // Act
        var result = await quoteService.CreateQuote(request);

        // Assert
        Assert.True(result.Success);
        var quote = result.Value; 
        
        Assert.NotNull(quote);
        Assert.NotEqual(Guid.Empty, quote.QuoteId);
        Assert.Equal(expectedInverseRate, quote.InverseOfxRate);
        Assert.Equal(expectedConvertedAmount, quote.ConvertedAmount);
    }
    
    [Fact]
    public void GetQuoteById_WhenNotFound_ReturnsFailure()
    {
        // Arrange
        var quoteId = Guid.NewGuid();
        _mockQuoteStore.Setup(s => s.Get(quoteId)).Returns((CreateQuoteResponse)null!);

        // Act
        var result = _service.GetQuoteById(quoteId);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("not found", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GetQuoteById_WhenFound_ReturnsSuccess()
    {
        // Arrange
        var quoteId = Guid.NewGuid();
        var expected = new CreateQuoteResponse { QuoteId = quoteId };
        _mockQuoteStore.Setup(s => s.Get(quoteId)).Returns(expected);

        // Act
        var result = _service.GetQuoteById(quoteId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expected, result.Value);
    }
}