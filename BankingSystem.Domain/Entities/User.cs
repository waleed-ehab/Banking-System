using BankingSystem.Domain.Enums;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.ValueObjects;

namespace BankingSystem.Domain.Entities;

public abstract class User
{
    public string Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string FullName => string.Concat(FirstName, " ", LastName);
    public Phone Phone { get; }
    public Email Email { get; }
    public Username Username { get; }
    public Password Password { get; }
    public UserRole Role { get; }
    public Permissions Permissions { get; protected set; }


    protected User(
        string id,
        string firstName,
        string lastName,
        Phone phone,
        Email email,
        Username username,
        Password password,
        UserRole role,
        Permissions permissions)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new DomainException("Id cannot be empty.");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be empty.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be empty.");

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        Username = username;
        Password = password;
        Role = role;
        Permissions = permissions;
    }
}
