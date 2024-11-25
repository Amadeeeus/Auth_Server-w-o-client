using Authorization.Application.Responses;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using MediatR;

namespace Authorization.Application.UseCases.Commands.Insert;

public record class LoginRequest:IRequest<AuthResponceDTO>
{
    public LoginEntity Entity { get; set; }
}
