using Application.Command;
using Application.Handlers;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class authController : ControllerBase
    {
        private readonly RegisterUserHandler _registerHandler;
        private readonly LoginUserHandler _loginHandler;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AppDbContext _db;
        private readonly IJwtTokenGenerator _jwt;

        public authController(
            RegisterUserHandler registerHandler,
            LoginUserHandler loginHandler,
            IRefreshTokenService refreshTokenService,
            AppDbContext db,
            IJwtTokenGenerator jwt)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
            _refreshTokenService = refreshTokenService;
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var result = await _registerHandler.Handle(command);
            return result ? Ok("Kayıt başarılı. Aktivasyon linki gönderildi.") : BadRequest("Bu email zaten kullanılıyor.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command)
        {
            var result = await _loginHandler.Handle(command);
            return result is not null ? Ok(result) : Unauthorized("Geçersiz giriş.");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || !await _refreshTokenService.ValidateRefreshTokenAsync(user.Id, request.RefreshToken))
                return Unauthorized();

            var newAccessToken = _jwt.Generate(user);
            var newRefreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(user.Id);

            return Ok(new
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return NotFound();

            var tokens = _db.RefreshTokens.Where(r => r.UserId == user.Id);
            _db.RefreshTokens.RemoveRange(tokens);
            await _db.SaveChangesAsync();

            return Ok("Çıkış yapıldı.");
        }
    }

    public record RefreshRequest(string Email, string RefreshToken);
    public record LogoutRequest(string Email);
}
