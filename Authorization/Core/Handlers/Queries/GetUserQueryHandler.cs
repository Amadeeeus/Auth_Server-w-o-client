﻿using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.Interfaces;
using Authorization.Infrastructure.Repositories;
using AutoMapper;
using MediatR;

namespace Authorization.Core.Handlers.Queries;

public class GetUserQueryHandler:IRequestHandler<GetUserRequest, UserDto>
{
    private readonly ILogger<GetUserQueryHandler> _logger;
    private readonly IAuthRepository _authRepository;
    private readonly IDistributedCacheRepository _cache;
    private readonly IHttpContextAccessor _accessor;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(ILogger<GetUserQueryHandler> logger, IAuthRepository authRepository,
        IDistributedCacheRepository cache, IHttpContextAccessor accessor, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _authRepository = authRepository;
        _cache = cache;
        _accessor = accessor;
    }

    public async Task<UserDto> Handle (GetUserRequest request, CancellationToken token)
    {
        var id = _accessor.HttpContext!.Request.Cookies["userId"];
        _logger.LogInformation($"GetUserQueryHandler::Handler::Id: {id}");
         var cache =await _cache.GetCacheAsync(id!);
         if (cache != null)
         {
             _logger.LogInformation($"returned user with Id: {id} from cache");
             return _mapper.Map<UserDto>(cache);
         }
         _logger.LogInformation($"User:{id} not found in cache: Going to repository");
        var user = await _authRepository.GetUser(id!, token);
        _logger.LogInformation($"User:{id} found in repository:");
        var addCache = _mapper.Map<UserDto>(user);
        var result = await _authRepository.GetUserByEmail(user.Email, token);
        await _cache.AddCacheAsync(addCache);
        _logger.LogInformation($"User:{id} added in cache:");
        return result;
    }
}