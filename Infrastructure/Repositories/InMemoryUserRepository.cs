using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    // Infrastructure/Repositories/InMemoryUserRepository.cs
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public Task<User?> GetByEmailAsync(string email)
            => Task.FromResult(_users.FirstOrDefault(u => u.Email == email));

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }
    }

}
