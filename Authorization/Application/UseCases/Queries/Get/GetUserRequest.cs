using Authorization.Core.DTOs;
using MediatR;


namespace Authorization.Application.UseCases.Queries.Get;

public record GetUserRequest : IRequest<GetUserDto>;
