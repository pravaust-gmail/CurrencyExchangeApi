using Moq;
using CurrencyExchangeApi.Services;
using CurrencyExchangeApi.Models.Requests;
using CurrencyExchangeApi.Models.Responses;
using CurrencyExchangeApi.Infrastructure;

namespace CurrencyExchangeApi.Tests.Services;

public class TransferServiceTests
{
    private readonly Mock<IStore<CreateQuoteResponse>> _mockQuoteStore;
    private readonly Mock<IStore<CreateTransferResponse>> _mockTransferStore;
    private readonly TransferService _service;

    public TransferServiceTests()
    {
        _mockQuoteStore = new Mock<IStore<CreateQuoteResponse>>();
        _mockTransferStore = new Mock<IStore<CreateTransferResponse>>();
        _service = new TransferService(_mockQuoteStore.Object, _mockTransferStore.Object);
    }
    
    [Fact]
    public void CreateTransfer_WhenQuoteNotFound_ReturnsFailure()
    {
        // Arrange
        var request = new CreateTransferRequest { QuoteId = Guid.NewGuid() };

        _mockQuoteStore
            .Setup(store => store.Get(request.QuoteId))
            .Returns((CreateQuoteResponse)null!);

        // Act
        var result = _service.CreateTransfer(request);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("not found", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CreateTransfer_WhenQuoteFound_SavesAndReturnsSuccess()
    {
        // Arrange
        var quoteId = Guid.NewGuid();
        var payerId = Guid.NewGuid();
        var request = new CreateTransferRequest
        {
            QuoteId = quoteId,
            Payer = new PayerDetailsRequest
            {
                Id = payerId,
                Name = "John Smith",
                TransferReason = "Personal"
            },
            Recipient = new RecipientDetailsRequest
            {
                Name = "Jane Smith",
                AccountNumber = "12345678",
                BankCode = "XYZ123",
                BankName = "CBA"
            }
        };

        var quote = new CreateQuoteResponse { QuoteId = quoteId };
        _mockQuoteStore.Setup(s => s.Get(quoteId)).Returns(quote);

        // Act
        var beforeTransfer = DateTime.UtcNow.AddSeconds(-1);
        var result = _service.CreateTransfer(request);
        var afterTransfer = DateTime.UtcNow.AddSeconds(1);

        // Assert
        Assert.True(result.Success);
        var transfer = result.Value; 

        Assert.NotNull(transfer);
        Assert.NotEqual(Guid.Empty, transfer.TransferId);
        Assert.Equal(TransferStatus.Processing, transfer.Status);
        Assert.InRange(transfer.EstimatedDeliveryDate, beforeTransfer.AddDays(1), afterTransfer.AddDays(1));
        
        Assert.NotNull(transfer.TransferDetails);
        Assert.Equal(quoteId, transfer.TransferDetails.QuoteId);

        Assert.NotNull(transfer.TransferDetails.Payer);
        Assert.Equal(payerId, transfer.TransferDetails.Payer.Id);
        Assert.Equal("John Smith", transfer.TransferDetails.Payer.Name);
        Assert.Equal("Personal", transfer.TransferDetails.Payer.TransferReason);

        Assert.NotNull(transfer.TransferDetails.Recipient);
        Assert.Equal("Jane Smith", transfer.TransferDetails.Recipient.Name);
        Assert.Equal("12345678", transfer.TransferDetails.Recipient.AccountNumber);
        Assert.Equal("XYZ123", transfer.TransferDetails.Recipient.BankCode);
        Assert.Equal("CBA", transfer.TransferDetails.Recipient.BankName);
    }
    
    [Fact]
    public void GetTransferById_WhenNotFound_ReturnsFailure()
    {
        // Arrange
        var transferId = Guid.NewGuid();
        _mockTransferStore.Setup(s => s.Get(transferId)).Returns((CreateTransferResponse)null!);

        // Act
        var result = _service.GetTransferById(transferId);

        // Assert
        Assert.False(result.Success);
        Assert.Contains("not found", result.ErrorMessage, StringComparison.OrdinalIgnoreCase);
    }
    
    [Fact]
    public void GetTransferById_WhenFound_ReturnsSuccess()
    {
        // Arrange
        var transferId = Guid.NewGuid();
        var expected = new CreateTransferResponse { TransferId = transferId };
        _mockTransferStore.Setup(s => s.Get(transferId)).Returns(expected);

        // Act
        var result = _service.GetTransferById(transferId);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(expected, result.Value);
    }
}