using System.ComponentModel.DataAnnotations;

namespace CurrencyExchangeApi.Models.Requests;

public class CreateTransferRequest
{
    [Required]
    public Guid QuoteId { get; set; }

    [Required]
    public PayerDetailsRequest Payer { get; set; } = null!;

    [Required]
    public RecipientDetailsRequest Recipient { get; set; } = null!;
}

public class PayerDetailsRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string TransferReason { get; set; } = null!;
}

public class RecipientDetailsRequest
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string AccountNumber { get; set; } = null!;

    [Required]
    public string BankCode { get; set; } = null!;

    [Required]
    public string BankName { get; set; } = null!;
}
