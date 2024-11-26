using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using MediatR;

namespace Authorization.Application.UseCases.Commands.Create;

public record class LoginUserRequest:IRequest
{
   public LoginEntity Entity { get; set; }
}