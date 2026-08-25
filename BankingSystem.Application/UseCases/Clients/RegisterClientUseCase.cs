using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Clients;

public record RegisterClientRequest(string FirstName, string LastName, string Username, string Password, string Email, string Phone);

public record RegisterClientResponse(string Id, string FirstName, string LastName, string Username, string Email, string Phone, string Permissions);

public class RegisterClientUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUser _currentUser;

    public RegisterClientUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _currentUser = currentUser;
    }

    public RegisterClientResponse Execute(RegisterClientRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var user = _userRepository.GetByUsername(request.Username.ToLowerInvariant());

        if (user is not null)
            throw new AlreadyExistsException("Username is already in use.");

        var hash = _passwordHasher.Hash(request.Password);

        var registeredUser = Client.Create(
            request.FirstName,
            request.LastName,
            Phone.Create(request.Phone),
            Email.Create(request.Email),
            Username.Create(request.Username),
            Password.FromHash(hash)
        );

        _userRepository.Save(registeredUser);

        return new RegisterClientResponse(
            registeredUser.Id,
            registeredUser.FirstName,
            registeredUser.LastName,
            registeredUser.Username.Value,
            registeredUser.Email.Value,
            registeredUser.Phone.Value,
            registeredUser.Permissions.ToString()
        );
    }
}
