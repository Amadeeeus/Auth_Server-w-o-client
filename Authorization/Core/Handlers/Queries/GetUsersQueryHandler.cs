
using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.Interfaces;
using Authorization.Infrastructure.Repositories;
using AutoMapper;
using MediatR;


namespace Authorization.Core.Handlers.Queries;

public class GetUsersQueryHandler:IRequestHandler<GetUsersRequest, List<GetUserDto>>
{
    private readonly ILogger<GetUsersQueryHandler> _logger;
    private readonly IDistributedCacheRepository _cache;
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IDistributedCacheRepository cache, IAuthRepository authRepository, IMapper mapper)
    {
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
        _authRepository = authRepository;
    }

    public async Task<List<GetUserDto>> Handle(GetUsersRequest request, CancellationToken ct)
    {
        var result = await _authRepository.GetUsers(ct);
        if (result == null || !result.Any())
        {
            _logger.LogWarning("No users found in the database.");
            return null!;
        }
        var cacheResult = _mapper.Map<List<UserDto>>(result);
        var cachedData = await _cache.GetCacheAsync();
        if (cachedData != null && cachedData.Any())
        {
            _logger.LogInformation($"Retrieved {cachedData.Count} users from cache.");
            return _mapper.Map<List<GetUserDto>>(cachedData);
        }
        await _cache.AddListCacheAsync(cacheResult);
        _logger.LogInformation("Users added to cache.");
        var users = _mapper.Map<List<GetUserDto>>(result);
        _logger.LogInformation($"Retrieved {result.Count} users from database.");
        return users;

    }
}