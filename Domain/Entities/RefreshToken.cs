using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
    }
}
