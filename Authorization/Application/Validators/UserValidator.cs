using Authorization.Core.Entities;
using Authorization.UI.Controllers;
using FluentValidation;

namespace Authorization.Application.Validators;

public class UserValidator: AbstractValidator<UserEntity>
{
    public UserValidator()
    {
        RuleFor(x=>x.FirstName).NotEmpty().WithMessage("Firstname cannot be empty");
        RuleFor(x=>x.LastName).Empty().WithMessage("Lastname cannot be empty");
        RuleFor(x=>x.Email).NotEmpty().WithMessage("Email cannot be empty");
        
        RuleFor(x=>x.FirstName).Length(1,16).WithMessage("First name must be between 1 and 16 characters");
        RuleFor(x=>x.LastName).Length(1,30).WithMessage("Last name must be between 1 and 30 characters");
        RuleFor(x=>x.Email).EmailAddress().WithMessage("Invalid email address");
        RuleFor(x=>x.Email).Length(1,255).WithMessage("Email must be between 1 and 255 characters");
    }
}