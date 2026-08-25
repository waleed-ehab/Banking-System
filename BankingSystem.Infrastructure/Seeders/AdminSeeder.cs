using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Infrastructure.Seeders;

public class AdminSeeder
{
    private readonly string _username = "admin";
    private readonly string _password = "1234";
    private readonly string _email = "lobo123@gmail.com";
    private readonly string _firstName = "Waleed";
    private readonly string _lastName = "Ehab";
    private readonly string _phone = "01238473343";

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AdminSeeder(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public void Seed()
    {
        var admins = _userRepository.GetAll()
            .OfType<Admin>();

        if (admins.Any())
            return;

        var hash = _passwordHasher.Hash(_password);

        var admin = Admin.Create(
            _firstName,
            _lastName,
            Phone.Create(_phone),
            Email.Create(_email),
            Username.Create(_username),
            Password.FromHash(hash)
        );

        _userRepository.Save(admin);
    }
}
