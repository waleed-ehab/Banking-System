using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Interfaces;

public interface IAccountRepository
{
    void Save(Account account);
    bool Delete(string id);
    Account? GetById(string id);
    IEnumerable<Account> GetAll();
    IEnumerable<Account> GetAllByClient(string clientId);
}
