using Authorization.Core.DTOs;
using Authorization.Core.Entities;

namespace Authorization.Infrastructure.Repositories;

public interface IDistributedCacheRepository
{
    Task AddCacheAsync(UserDto responce);
    Task AddListCacheAsync(List<UserDto> responce);
    Task<UserDto> GetCacheAsync(string id);
    Task DeleteCacheAsync(string id);
    Task<List<UserDto>> GetCacheAsync();
}