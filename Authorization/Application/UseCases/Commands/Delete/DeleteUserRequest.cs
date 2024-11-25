using Authorization.Application.Responses;
using MediatR;

namespace Authorization.Application.UseCases.Commands.Delete;

public record class DeleteUserRequest:IRequest;