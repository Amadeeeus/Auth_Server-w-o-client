using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Application.UseCases.Queries.Get;

public record class GetUsersRequest : IRequest<List<GetUserDto>>;
