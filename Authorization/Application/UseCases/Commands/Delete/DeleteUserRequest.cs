using Authorization.Application.Responses;
using MediatR;

namespace Authorization.Application.UseCases.Commands.Delete;

public record DeleteUserRequest:IRequest;