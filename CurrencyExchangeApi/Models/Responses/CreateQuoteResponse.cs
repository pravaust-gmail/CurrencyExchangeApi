namespace CurrencyExchangeApi.Models.Responses;

public class CreateQuoteResponse
{
    public Guid QuoteId { get; set; }
    public decimal OfxRate { get; set; }
    public decimal InverseOfxRate { get; set; }
    public decimal ConvertedAmount { get; set; }
}