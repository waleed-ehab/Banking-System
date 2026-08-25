using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Clients;

public record GetClientRequest(string Id);

public record GetClientResponse(string Id, string FirstName, string LastName, string Phone, string Email, string Username, string Permissions);

public class GetClientUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public GetClientUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public GetClientResponse Execute(GetClientRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var user = _userRepository.GetById(request.Id);

        if (user is not Client client)
            throw new NotFoundException($"Client with id '{request.Id}' not found.");

        return new GetClientResponse(
            client.Id,
            client.FirstName,
            client.LastName,
            client.Phone.Value,
            client.Email.Value,
            client.Username.Value,
            client.Permissions.ToString()
        );
    }
}
