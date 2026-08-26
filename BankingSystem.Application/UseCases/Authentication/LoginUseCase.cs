using BankingSystem.Application.Exceptions;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Enums;

namespace BankingSystem.Application.UseCases.Authentication;

public record LoginRequest(string Username, string Password);
public record LoginResponse(string Id, string FullName, UserRole Role, Permissions Permissions);

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public LoginResponse Execute(LoginRequest request)
    {
        var user = _userRepository.GetByUsername(request.Username)
            ?? throw new UnauthorizedException("Invalid credentials.");

        if (!_passwordHasher.Verify(request.Password, user.Password.Hash))
            throw new UnauthorizedException("Invalid credentials.");
        
        return new LoginResponse(
            user.Id,
            user.FullName,
            user.Role,
            user.Permissions
        );
    }
}
