using Authorization.Core.Entities;
using FluentValidation;

namespace Authorization.Application.Validators;

public class RegisterValidator : AbstractValidator<RegistrationEntity>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty");
        RuleFor(x=>x.Firstname).NotEmpty().WithMessage("First name cannot be empty");
        RuleFor(x=>x.Lastname).NotEmpty().WithMessage("Last name cannot be empty");
       RuleFor(x=>x.Password).Length(1,255).WithMessage("Password must be between 1 and 255 characters");
       RuleFor(x=>x.Email).EmailAddress().WithMessage("Invalid email address");
       RuleFor(x=>x.Email).Length(1,255).WithMessage("Email must be between 1 and 255 characters");
       RuleFor(x=>x.Firstname).Length(1,255).WithMessage("First name must be between 1 and 255 characters");
       RuleFor(x=>x.Lastname).Length(1,255).WithMessage("Last name must be between 1 and 255 characters");
       RuleFor(x=>x.Password).Length(1,255).WithMessage("Password must be between 1 and 255 characters");
    }
}