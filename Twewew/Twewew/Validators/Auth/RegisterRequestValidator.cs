using FluentValidation;
using Twewew.Requests.Auth;

namespace Twewew.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username cannot be empty");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty.")
            .Must((request, password) => request.ConfirmPassword.Equals(password))
            .WithMessage("Password and confirm password do not match.")
            .MinimumLength(8)
            .WithMessage("Password is too short.");


        RuleFor(x => x.ConfirmUrl)
              .NotEmpty()
             .WithMessage("Confirmation URL cannot be empty.");

        RuleFor(x => x.OS)
            .NotEmpty()
            .Unless(x => x.OS == null);

        RuleFor(x => x.Browser)
            .NotEmpty()
            .Unless(x => x.Browser == null);


    }
}
