using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;

namespace BankingSystem.Application.UseCases.Currencies;

public record GetCurrencyRequest(string Code);

public record GetCurrencyResponse(string Code, string Country, string Name, decimal Rate);

public class GetCurrencyUseCase
{
    private readonly ICurrencyRepository _currencyRepository;

    public GetCurrencyUseCase(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public GetCurrencyResponse Execute(GetCurrencyRequest request)
    {
        var currency = _currencyRepository.GetByCode(request.Code.ToUpper());

        if (currency is null)
            throw new NotFoundException($"Currency with code '{request.Code}' not found.");

        return new GetCurrencyResponse(
            currency.Code,
            currency.Country,
            currency.Name,
            currency.Rate
        );
    }
}
