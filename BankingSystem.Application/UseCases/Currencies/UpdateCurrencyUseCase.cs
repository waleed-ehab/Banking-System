using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Application.UseCases.Currencies;

public record UpdateCurrencyRequest(string Code, decimal Rate);

public record UpdateCurrencyResponse(string Code, string Country, string Name, decimal Rate);

public class UpdateCurrencyUseCase
{
    private readonly ICurrencyRepository _currencyRepository;
    private readonly ICurrentUser _currentUser;

    public UpdateCurrencyUseCase(ICurrencyRepository currencyRepository, ICurrentUser currentUser)
    {
        _currencyRepository = currencyRepository;
        _currentUser = currentUser;
    }

    public UpdateCurrencyResponse Execute(UpdateCurrencyRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var existing = _currencyRepository.GetByCode(request.Code.ToUpper());

        if (existing is null)
            throw new NotFoundException($"Currency with code '{request.Code}' not found.");

        var diff = Math.Abs(existing.Rate - request.Rate);

        if (diff > existing.Rate * 0.5m)
            throw new ValidationException("Exchange rate change is too extreme.");

        var updated = Currency.Create(existing.Country, existing.Code, existing.Name, request.Rate);

        _currencyRepository.Save(updated);

        return new UpdateCurrencyResponse(
            updated.Code,
            updated.Country,
            updated.Name,
            updated.Rate
        );
    }
}
