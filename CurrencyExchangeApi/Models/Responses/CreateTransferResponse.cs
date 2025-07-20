using System.Text.Json.Serialization;

namespace CurrencyExchangeApi.Models.Responses;

public class CreateTransferResponse
{
    public Guid TransferId { get; set; }
    public TransferStatus Status { get; set; }
    public TransferDetails TransferDetails { get; set; } = null!;
    public DateTime EstimatedDeliveryDate { get; set; }
}

public class TransferDetails
{
    public Guid QuoteId { get; set; }
    public PayerDetailsResponse Payer { get; set; } = null!;
    public RecipientDetailsResponse Recipient { get; set; } = null!;
}

public class PayerDetailsResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string TransferReason { get; set; } = null!;
}

public class RecipientDetailsResponse
{
    public string Name { get; set; } = null!;
    public string AccountNumber { get; set; } = null!;
    public string BankCode { get; set; } = null!;
    public string BankName { get; set; } = null!;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransferStatus
{
    Created,
    Processing,
    Processed,
    Failed
}
