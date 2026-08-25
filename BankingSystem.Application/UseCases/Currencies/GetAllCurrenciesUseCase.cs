using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Currencies;

public record GetAllCurrenciesResponse(IEnumerable<Currency> Currencies);

public class GetAllCurrenciesUseCase
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetAllCurrenciesUseCase(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public GetAllCurrenciesResponse Execute()
    {
        var currencies = _currencyRepository.GetAll();
        return new GetAllCurrenciesResponse(currencies);
    }
}
