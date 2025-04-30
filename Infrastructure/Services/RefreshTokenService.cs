using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    // Infrastructure/Services/RefreshTokenService.cs
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(Guid userId);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string token);
    }

    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _context;

        public RefreshTokenService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                Expiration = DateTime.UtcNow.AddDays(7)
            };
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string token)
        {
            var saved = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.UserId == userId && r.Token == token && r.Expiration > DateTime.UtcNow);
            return saved != null;
        }
    }

}
