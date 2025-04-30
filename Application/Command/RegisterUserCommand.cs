using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    // Application/Commands/RegisterUserCommand.cs
    public record RegisterUserCommand(string Email, string Password, string Role);

}
