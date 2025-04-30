using System;

// Domain/Entities/User.cs
public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Email { get; private set; }
    public string PasswordHash { get; private set; } // artık hash
    public string Role { get; private set; } // admin veya guide

    public User(string email, string passwordHash, string role)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}
