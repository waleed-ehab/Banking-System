namespace BankingSystem.Infrastructure.DataModels;

public class TransferDataModel
{
    public string SourceAccountId { get; set; } = string.Empty;
    public string DestinationAccountId { get; set; } = string.Empty;
    public decimal Amount { get; set; } = 0.0m;
    public string CurrencyCode { get; set; } = string.Empty; 
    public DateTime Date { get; set; }
}
    