using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using MediatR;


namespace Authorization.Application.UseCases.Queries.Get;

public record GetUserRequest : IRequest<UserDto>;
