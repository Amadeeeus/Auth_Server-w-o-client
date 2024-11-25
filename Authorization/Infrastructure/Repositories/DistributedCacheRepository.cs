using System.Text.Json;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;

namespace Authorization.Infrastructure.Repositories;

public class DistributedCacheRepository : IDistributedCacheRepository
{        
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<DistributedCacheRepository> _logger;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public DistributedCacheRepository(IDistributedCache distributedCache, ILogger<DistributedCacheRepository> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _distributedCache = distributedCache;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task AddCacheAsync(UserDto responce)
    {
        var context = _httpContextAccessor.HttpContext;
        var id = context!.Request.Cookies["Id"];
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
        };
        var serialized =JsonSerializer.Serialize(responce);
        await _distributedCache.SetStringAsync($"user: {id}",serialized,cacheOptions);
    }

    public async Task AddListCacheAsync(List<UserDto> responce)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
        };
        var serialized =JsonSerializer.Serialize(responce);
        await _distributedCache.SetStringAsync("user-list", serialized, cacheOptions); 
    }

    public async Task<UserDto> GetCacheAsync(string id)
    {
        var cache = await _distributedCache.GetStringAsync($"user: {id}");
        if (cache == null)
        {
            return null!;
        }
        var unserialized = JsonSerializer.Deserialize<UserDto>(cache);
        return unserialized!;
    }

    public async Task<List<UserDto>> GetCacheAsync()
    {
        var cache = await _distributedCache.GetStringAsync("user-list");
        if (cache == null)
        {
            return null;
        }
        var unserialized = JsonSerializer.Deserialize<List<UserDto>>(cache);
        return unserialized!;

    }

    public async Task DeleteCacheAsync(string id)
    {
        await  _distributedCache.RemoveAsync($"user: {id}");
    }
}