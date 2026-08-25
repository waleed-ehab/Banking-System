using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Transactions;

public record TransferSummary(string SourceAccountId, string DestinationAccountId, decimal Amount, string CurrencyCode, DateTime OccurredAt);

public record GetAllTransfersResponse(IEnumerable<TransferSummary> Transfers);

public class GetAllTransfersUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransferRepository _transferRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllTransfersUseCase(IAccountRepository accountRepository, ITransferRepository transferRepository, ICurrentUser currentUser)
    {
        _accountRepository = accountRepository;
        _transferRepository = transferRepository;
        _currentUser = currentUser;
    }

    public GetAllTransfersResponse Execute()
    {
        if (!_currentUser.Permissions.HasFlag(Permissions.Read))
            throw new UnauthorizedException("You do not have permission to perform this action.");

        IEnumerable<Transfer> transfers;

        if (_currentUser.Role == UserRole.Admin)
        {
            transfers = _transferRepository.GetAll();
        }
        else
        {
            var accountIds = _accountRepository.GetAll()
                .Where(a => a.UserId == _currentUser.Id)
                .Select(a => a.AccountId);

            transfers = _transferRepository.GetByAccountIds(accountIds);
        }

        var summaries = transfers.Select(t => new TransferSummary(
            t.SourceAccountId,
            t.DestinationAccountId,
            t.Amount.Amount,
            t.Amount.CurrencyCode,
            t.OccurredAt
        ));

        return new GetAllTransfersResponse(summaries);
    }
}
