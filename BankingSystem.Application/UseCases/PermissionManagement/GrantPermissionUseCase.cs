using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.PermissionManagement;

public record GrantPermissionRequest(string ClientId, Permissions Permission);

public record GrantPermissionResponse(string ClientId, Permissions UpdatedPermissions);

public class GrantPermissionUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public GrantPermissionUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public GrantPermissionResponse Execute(GrantPermissionRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var user = _userRepository.GetById(request.ClientId);

        if (user is not Client client)
            throw new NotFoundException($"Client with id '{request.ClientId}' not found.");

        client.Grant(request.Permission);

        _userRepository.Save(client);

        return new GrantPermissionResponse(
            client.Id,
            client.Permissions
        );
    }
}
