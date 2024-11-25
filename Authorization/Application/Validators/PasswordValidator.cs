using Authorization.Core.Entities;
using FluentValidation;

namespace Authorization.Application.Validators;

public class PasswordValidator:AbstractValidator<PasswordEntity>
{
    public PasswordValidator()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
        RuleFor(x => x.Password).Length(1, 255).WithMessage("password must be between 1 and 255 characters");
    }
}