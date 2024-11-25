
using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
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
        var cacheResult = _mapper.Map<List<UserDto>>(result);
        if (result.Count == 0)
        {
            return null!;
        }
        _logger.LogInformation($"Retrieved {result.Count} users");
        var cached = _cache.GetCacheAsync();
        var returnedCache = _mapper.Map<List<UserDto>, List<GetUserDto>>(cached.Result);
        if (cached.IsCompletedSuccessfully)
        {
            _logger.LogInformation($"Retrieved {result.Count} users from cache");
            return returnedCache;
        }
        await _cache.AddListCacheAsync(cacheResult);
        var users = _mapper.Map<List<UserEntity>, List<GetUserDto>>(result);
        _logger.LogInformation("users added in cache");
        return users;
    }
}