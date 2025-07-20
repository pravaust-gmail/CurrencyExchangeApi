namespace CurrencyExchangeApi.Models.External;

public class FrankfurterResponse
{
    public string Base { get; set; } = null!;
    public DateTime Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; } = null!;
}