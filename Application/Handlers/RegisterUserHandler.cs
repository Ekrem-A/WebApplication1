using Application.Command;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    // Application/Handlers/RegisterUserHandler.cs
    public class RegisterUserHandler
    {
        private readonly IUserRepository _repository;

        public RegisterUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RegisterUserCommand command)
        {
            var existing = await _repository.GetByEmailAsync(command.Email);
            if (existing is not null) return false;

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var user = new User(command.Email, passwordHash, command.Role.ToLower()); // rol küçük harf olsun
            await _repository.AddAsync(user);
            return true;
        }
    }

}
