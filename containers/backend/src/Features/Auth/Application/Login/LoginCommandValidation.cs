using FluentValidation;
using PrzepisakApi.api.src.Features.Auth.Application.Login;

namespace miejsce.api.src.Features.Auth.Application.Login;

public class LoginCommandValidation : AbstractValidator<LoginCommand>
{
    public LoginCommandValidation()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Podaj nazwę użytkownika");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Podaj hasło");
    }

}
