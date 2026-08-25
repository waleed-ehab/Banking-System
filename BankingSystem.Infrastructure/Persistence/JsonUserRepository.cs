using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Enums;
using BankingSystem.Domain.ValueObjects;
using BankingSystem.Domain.Converters;
using BankingSystem.Domain.DataModels;
using System.Text.Json;

namespace BankingSystem.Domain.Persistence;

public class JsonUserRepository : IUserRepository
{
    private readonly string _filePath;

    public JsonUserRepository(string filePath)
    {
        _filePath = filePath;
    }

    public bool Delete(string id)
    {
        var users = GetAll().ToList();

        if (users.RemoveAll(u => u.Id == id) == 0)
        {
            return false;
        }

        WriteToFile(_filePath, users);

        return true;
    }

    public IEnumerable<User> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(_filePath);
        var dataModels = JsonSerializer.Deserialize<IEnumerable<UserDataModel>>(json)
            ?? new List<UserDataModel>();

        return dataModels.Select(ToDomain);
    }

    public User? GetById(string id)
    {
        var users = GetAll().ToList();
        return users.FirstOrDefault(u => u.Id == id);
    }

    public User? GetByUsername(string username)
    {
        var users = GetAll().ToList();
        return users.FirstOrDefault(u => u.Username.Value == username);
    }

    public void Save(User user)
    {
        var users = GetAll().ToList();
        var index = users.FindIndex(u => u.Id == user.Id);

        if (index >= 0)
        {
            users[index] = user;
        }
        else
        {
            users.Add(user);
        }

        WriteToFile(_filePath, users);
    }

    private static void WriteToFile(string path, IEnumerable<User> users)
    {
        var dataModels = users.Select(ToDataModel);
        var json = JsonSerializer.Serialize(
            dataModels,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(path, json);
    }

    private static UserDataModel ToDataModel(User user)
    {
        return new UserDataModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            Email = user.Email.Value,
            Phone = user.Phone.Value,
            PasswordHash = user.Password.Hash,
            Permissions = PermissionConverter.ToRwx(user.Permissions),
            Role = user.Role.ToString(),
            Username = user.Username.Value
        };
    }

    private static User ToDomain(UserDataModel dataModel)
    {
        return dataModel.Role switch
        {
            nameof(UserRole.Admin) => Admin.Rehydrate(
                dataModel.Id,
                dataModel.FirstName,
                dataModel.LastName,
                Phone.Create(dataModel.Phone),
                Email.Create(dataModel.Email),
                Username.Create(dataModel.Username),
                Password.FromHash(dataModel.PasswordHash),
                Enum.Parse<UserRole>(dataModel.Role),
                PermissionConverter.FromRwx(dataModel.Permissions)
            ),
            nameof(UserRole.Client) => Client.Rehydrate(
                dataModel.Id,
                dataModel.FirstName,
                dataModel.LastName,
                Phone.Create(dataModel.Phone),
                Email.Create(dataModel.Email),
                Username.Create(dataModel.Username),
                Password.FromHash(dataModel.PasswordHash),
                Enum.Parse<UserRole>(dataModel.Role),
                PermissionConverter.FromRwx(dataModel.Permissions)),
            _ => throw new InvalidOperationException($"Unknown role: {dataModel.Role}")
        };
    }
}
