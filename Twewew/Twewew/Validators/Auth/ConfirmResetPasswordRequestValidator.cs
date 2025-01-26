﻿using FluentValidation;
using Twewew.Requests.Auth;

namespace Twewew.Validators.Auth;

public class ConfirmResetPasswordRequestValidator : AbstractValidator<ConfirmResetPasswordRequest>
{
    public ConfirmResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .EmailAddress()
            .WithMessage("Invalid email address");

        RuleFor(x => x.NewPassword)
                  .NotEmpty()
                  .WithMessage("Password cannot be empty.")
                  .Must((request, password) => request.ConfirmNewPassword.Equals(password))
                  .WithMessage("Passwords are do not match.")
                  .MinimumLength(8)
                  .WithMessage("Password is too short.");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token cannot be empty");
    }
}
