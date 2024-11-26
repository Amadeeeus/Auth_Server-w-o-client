using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using MediatR;

namespace Authorization.Application.UseCases.Commands.Insert;

public record PutUserRequest : IRequest
{ 
    public RegistrationEntity Entity { get; init; }
}
