using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.PermissionManagement;

public record ResetClientPermissionsRequest(string ClientId);

public record ResetClientPermissionsResponse(string ClientId, Permissions UpdatedPermissions);

public class ResetClientPermissionsUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public ResetClientPermissionsUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public ResetClientPermissionsResponse Execute(ResetClientPermissionsRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var user = _userRepository.GetById(request.ClientId);

        if (user is not Client client)
            throw new NotFoundException($"Client with id '{request.ClientId}' not found.");

        client.ResetToDefault();

        _userRepository.Save(client);

        return new ResetClientPermissionsResponse(
            client.Id,
            client.Permissions
        );
    }
}
