using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Converters;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Clients;

public record ClientSummary(string Id, string FullName, string Phone, string Email, string Username, string Permissions);

public record GetAllClientsResponse(IEnumerable<ClientSummary> Clients);

public class GetAllClientsUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public GetAllClientsUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public GetAllClientsResponse Execute()
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");
        
        var clients = _userRepository.GetAll()
            .OfType<Client>()
            .Select(c => new ClientSummary(
                c.Id,
                c.FullName,
                c.Phone.Value,
                c.Email.Value,
                c.Username.Value,
                PermissionConverter.ToRwx(c.Permissions)
            ));

        return new GetAllClientsResponse(clients);
    }
}
