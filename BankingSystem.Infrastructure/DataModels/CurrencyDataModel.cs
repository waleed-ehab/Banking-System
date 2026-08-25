namespace BankingSystem.Infrastructure.DataModels;

public class CurrencyDataModel
{
    public string Country { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; } = 0.0m;
}
