using BankingSystem.Application.Common;
using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Application.UseCases.Clients;

public record UpdateClientRequest(string Id, string FirstName, string LastName, string Phone, string Email);

public record UpdateClientResponse(string Id, string FirstName, string LastName, string Phone, string Email, string Username, string Permissions);

public class UpdateClientUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public UpdateClientUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public UpdateClientResponse Execute(UpdateClientRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var existing = _userRepository.GetById(request.Id);

        if (existing is not Client client)
            throw new NotFoundException($"Client with id '{request.Id}' not found.");

        var updated = Client.Rehydrate(
            client.Id,
            request.FirstName,
            request.LastName,
            Phone.Create(request.Phone),
            Email.Create(request.Email),
            client.Username,
            client.Password,
            client.Role,
            client.Permissions
        );

        _userRepository.Save(updated);

        return new UpdateClientResponse(
            updated.Id,
            updated.FirstName,
            updated.LastName,
            updated.Phone.Value,
            updated.Email.Value,
            updated.Username.Value,
            updated.Permissions.ToString()
        );
    }
}
