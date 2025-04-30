using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Application.Command;
using Infrastructure.Repositories;
using Contracts.DTOs;
using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using Infrastructure.Services;

namespace Application.Handlers
{
    public class LoginUserHandler
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenGenerator _tokenGenerator;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginUserHandler(IUserRepository repository, IJwtTokenGenerator tokenGenerator, IRefreshTokenService refreshTokenService)
        {
            _repository = repository;
            _tokenGenerator = tokenGenerator;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponseDto?> Handle(LoginUserCommand command)
        {
            var user = await _repository.GetByEmailAsync(command.Email);
            if (user == null || !user.VerifyPassword(command.Password)) return null;

            var accessToken = _tokenGenerator.Generate(user);
            var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return new LoginResponseDto(accessToken, user.Email, refreshToken);
        }
    }

}
