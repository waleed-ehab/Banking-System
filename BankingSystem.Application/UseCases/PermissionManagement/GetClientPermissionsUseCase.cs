using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.PermissionManagement;

public record GetClientPermissionsRequest(string ClientId);

public record GetClientPermissionsResponse(string ClientId, string FullName, Permissions Permissions);

public class GetClientPermissionsUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public GetClientPermissionsUseCase(IUserRepository userRepository, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
    }

    public GetClientPermissionsResponse Execute(GetClientPermissionsRequest request)
    {
        if (_currentUser.Role != UserRole.Admin)
            throw new UnauthorizedException("You do not have permission to perform this action.");

        var user = _userRepository.GetById(request.ClientId);

        if (user is not Client client)
            throw new NotFoundException($"Client with id '{request.ClientId}' not found.");

        return new GetClientPermissionsResponse(
            client.Id,
            client.FullName,
            client.Permissions
        );
    }
}
