using Authorization.Application.Responses;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using MediatR;

namespace Authorization.Application.UseCases.Commands.Create;

public record class RegisterUserRequest : IRequest
{
    public RegistrationEntity Entity { get; init; }
}