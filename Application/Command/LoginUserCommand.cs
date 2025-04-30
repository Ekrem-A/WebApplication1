using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Command
{
    // Application/Commands/LoginUserCommand.cs
    public record LoginUserCommand(string Email, string Password);

}
