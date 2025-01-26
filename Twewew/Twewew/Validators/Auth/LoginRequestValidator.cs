using FluentValidation;
using Twewew.Requests.Auth;


namespace Twewew.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
        .WithMessage("Username cannot be empty")
        .MaximumLength(50).WithMessage("Username cannot exceed 100 characters");


        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}
