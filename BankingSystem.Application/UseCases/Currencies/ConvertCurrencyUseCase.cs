using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;

namespace BankingSystem.Application.UseCases.Currencies;

public record ConvertCurrencyRequest(string FromCode, string ToCode, decimal FromAmount);

public record ConvertCurrencyResponse(string FromName, string ToName, decimal ToAmount);

public class ConvertCurrencyUseCase
{
    private readonly ICurrencyRepository _currencyRepository;

    public ConvertCurrencyUseCase(ICurrencyRepository currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public ConvertCurrencyResponse Execute(ConvertCurrencyRequest request)
    {
        var fromCurrency = _currencyRepository.GetByCode(request.FromCode.ToUpper())
            ?? throw new NotFoundException($"From currency with code '{request.FromCode}' not found.");
        var toCurrency = _currencyRepository.GetByCode(request.ToCode.ToUpper())
            ?? throw new NotFoundException($"To currency with code '{request.ToCode}' not found.");

        if (request.FromAmount < 0)
            throw new InvalidOperationException($"Amount to convert cannot be negative.");

        var convertedAmount =  request.FromAmount * (toCurrency.Rate / fromCurrency.Rate);

        return new ConvertCurrencyResponse(
            fromCurrency.Name,
            toCurrency.Name,
            convertedAmount
        );
    }
}
