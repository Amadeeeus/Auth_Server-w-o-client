using Authorization.Core.DTOs;
using Authorization.Core.Entities;

namespace Authorization.Infrastructure.Repositories;

public interface IAuthRepository
{
    Task Create(UserEntity entity,PasswordEntity password,  CancellationToken cancellationToken);
    Task Delete(string userId, CancellationToken cancellationToken);
    Task<AuthResponceDTO> Update(UserEntity entity, CancellationToken cancellationToken);
    Task<UserEntity> GetUser(string userId, CancellationToken cancellationToken);
    Task<List<UserEntity>> GetUsers(CancellationToken cancellationToken);
    Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken);
}