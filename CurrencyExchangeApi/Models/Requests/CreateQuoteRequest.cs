using System.ComponentModel.DataAnnotations;

namespace CurrencyExchangeApi.Models.Requests;

public class CreateQuoteRequest
{
    [Required]
    public string SellCurrency { get; set; } = null!;

    [Required]
    public string BuyCurrency { get; set; } = null!;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
}