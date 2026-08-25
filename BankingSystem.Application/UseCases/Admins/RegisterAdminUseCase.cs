using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Admins;

public record RegisterAdminRequest(string FirstName, string LastName, string Username, string Password, string Email, string Phone, string Role);

public record RegisterAdminResponse(string Id, string FirstName, string LastName, string Username, string Email, string Phone, string permissions);

public class RegisterAdminUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterAdminUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public RegisterAdminResponse Execute(RegisterAdminRequest request)
    {
        var user = _userRepository.GetByUsername(request.Username.ToLowerInvariant());

        if (user is not null)
            throw new AlreadyExistsException("Username is already in use.");

        var hash = _passwordHasher.Hash(request.Password);

        var newCreatedUser = Admin.Create(
            request.FirstName,
            request.LastName,
            Phone.Create(request.Phone),
            Email.Create(request.Email),
            Username.Create(request.Username),
            Password.FromHash(hash)
        );

        _userRepository.Save(newCreatedUser);

        return new RegisterAdminResponse(
            newCreatedUser.Id,
            newCreatedUser.FirstName,
            newCreatedUser.LastName,
            newCreatedUser.Username.Value,
            newCreatedUser.Email.Value,
            newCreatedUser.Phone.Value,
            newCreatedUser.Permissions.ToString()
        );
    }
}
