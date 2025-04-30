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

        var user = new User(command.Email, command.Password); // şifre hashlenebilir
        await _repository.AddAsync(user);
        return true;
    }
}
