using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Infrastructure.Seeders;

public class CurrencySeeder
{
    private readonly ICurrencyRepository _currencyRepository;

    private static readonly IReadOnlyList<Currency> DefaultCurrencies =
    [
        Currency.Create("United States of America", "USD", "US Dollar", 1.000000m),
        Currency.Create("Afghanistan", "AFN", "Afghanistan Afghani", 87.480003m),
        Currency.Create("Albania", "ALL", "Albania Lek(e)", 109.010002m),
        Currency.Create("Algeria", "DZD", "Algerian Dinar", 137.046005m),
        Currency.Create("American Samoa", "USD", "US Dollar", 1.000000m),
        Currency.Create("France", "EUR", "Euro", 0.900000m),
        Currency.Create("Angola", "AOA", "Angolan Kwanza", 504.734009m),
        Currency.Create("Anguilla", "XCD", "E.C. Dollar", 2.700000m),
        Currency.Create("Antigua and Barbuda", "XCD", "E.C. Dollar", 2.700000m),
        Currency.Create("Argentina", "ARS", "Argentine Peso", 171.744995m),
        Currency.Create("Armenia", "AMD", "Armenian Dram", 391.700012m),
        Currency.Create("Aruba", "AWG", "Aruban Guilder", 1.790000m),
        Currency.Create("Australia", "AUD", "Australian Dollar", 1.451000m),
        Currency.Create("Austria", "EUR", "Euro", 0.938000m),
        Currency.Create("Azerbaijan", "AZN", "Azerbaijan Manat", 1.694000m),
        Currency.Create("Bahamas", "BSD", "Bahamian Dollar", 1.000000m),
        Currency.Create("Bahrain", "BHD", "Bahraini Dinar", 0.377000m),
        Currency.Create("Bangladesh", "BDT", "Bangladesh Taka", 101.000000m),
        Currency.Create("Barbados", "BBD", "Barbados Dollar", 2.000000m),
        Currency.Create("Belarus", "BYN", "Belarusian Ruble", 2.478000m),
        Currency.Create("Belgium", "EUR", "Euro", 0.938000m),
        Currency.Create("Belize", "BZD", "Belize Dollar", 2.000000m),
        Currency.Create("Benin", "XOF", "CFA Franc", 615.401978m),
        Currency.Create("Bermuda", "BMD", "Bermuda Dollar", 1.000000m),
        Currency.Create("Bhutan", "BTN", "Bhutan Ngultrum", 82.800003m),
        Currency.Create("Bolivia", "BOB", "Bolivia Boliviano", 6.852000m),
        Currency.Create("Bosnia and Herzegovina", "BAM", "Bosnia and Herzegovina Convertible Mark", 1.835000m),
        Currency.Create("Botswana", "BWP", "Botswana Pula", 12.953000m),
        Currency.Create("Brazil", "BRL", "Brazilian Real", 5.261000m),
        Currency.Create("Brunei", "BND", "Brunei Dollar", 1.344000m),
        Currency.Create("Bulgaria", "BGN", "Bulgarian Lev", 1.835000m),
        Currency.Create("Burkina Faso", "XOF", "CFA Franc", 615.401978m),
        Currency.Create("Burundi", "BIF", "Burundi Franc", 2043.123047m),
        Currency.Create("Cambodia", "KHR", "Cambodian Riel", 4130.000000m),
        Currency.Create("Cameroon", "XAF", "CFA Franc", 615.401978m),
        Currency.Create("Canada", "CAD", "Canadian Dollar", 1.353000m),
        Currency.Create("Egypt", "EGP", "Egyptian Pound", 49.400002m),
        Currency.Create("Saudi Arabia", "SAR", "Saudi Riyal", 3.759000m),
        Currency.Create("United Arab Emirates", "AED", "United Arab Emirates Dirham", 3.673000m),
        Currency.Create("United Kingdom", "GBP", "U.K. Pound", 0.804000m),
        Currency.Create("Japan", "JPY", "Japanese Yen", 134.869995m),
        Currency.Create("China", "CNY", "Chinese Renminbi", 6.940000m),
        Currency.Create("India", "INR", "Indian Rupee", 82.800003m)
    ];

    public CurrencySeeder(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public void Seed()
    {
        _currencyRepository.InsertRange(DefaultCurrencies);
    }
}
