using BankingSystem.Domain.Entities;

namespace BankingSystem.Application.Interfaces;

public interface IUserRepository
{
    void Save(User user);
    bool Delete(string id);
    User? GetById(string id);
    User? GetByUsername(string username);
    IEnumerable<User> GetAll();
}
