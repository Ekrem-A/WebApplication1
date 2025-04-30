using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.DTOs
{
    // Contracts/DTOs/LoginResponseDto.cs
    public record LoginResponseDto(string Token, string Email, string RefreshToken);


}
