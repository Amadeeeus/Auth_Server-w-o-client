using Authorization.Core.DTOs;
using Authorization.Core.Entities;

namespace Authorization.Core.Interfaces;

public interface IAuthRepository
{
    Task Create(UserEntity entity,PasswordEntity password,  CancellationToken cancellationToken);
    Task Delete(string userId, CancellationToken cancellationToken);
    Task Update(UserDto entity, CancellationToken cancellationToken);
    Task<UserEntity> GetUser(string userId, CancellationToken cancellationToken);
    Task<List<UserEntity>> GetUsers(CancellationToken cancellationToken);
    Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken);
}