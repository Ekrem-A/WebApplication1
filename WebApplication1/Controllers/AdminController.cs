using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Services;
using System.Threading.Tasks;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("users")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            var users = _db.Users.Select(u => new { u.Email, u.Role, u.IsActive }).ToList();
            return Ok(users);
        }

        [HttpPost("deactivate")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeactivateUser([FromBody] string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            user.IsActive = false;
            await _db.SaveChangesAsync();

            return Ok("Kullanıcı pasif yapıldı.");
        }
    }
}
