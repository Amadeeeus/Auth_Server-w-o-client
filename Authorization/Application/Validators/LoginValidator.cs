using Authorization.Core.Entities;
using FluentValidation;

namespace Authorization.Application.Validators;

public class LoginValidator:AbstractValidator<LoginEntity>
{
    public LoginValidator()
    {
        RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x=>x.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(x=>x.Email).Length(1,255).WithMessage("Email must be between 1 and 255 characters");
        RuleFor(x=>x.Email).EmailAddress().WithMessage("Email is invalid");
        RuleFor(x=>x.Password).Length(1,255).WithMessage("Password must be between 1 and 255 characters");
    }
}