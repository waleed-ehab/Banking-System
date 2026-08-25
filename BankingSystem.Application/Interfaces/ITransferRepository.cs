using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Interfaces;

public interface ITransferRepository
{
    IEnumerable<Transfer> GetAll();
    IEnumerable<Transfer> GetByAccountIds(IEnumerable<string> accountIds);
    void Save(Transfer transfer);
}
