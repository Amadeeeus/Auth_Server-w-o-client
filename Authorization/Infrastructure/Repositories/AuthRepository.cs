using Authorization.Application.UseCases.Commands.Delete;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.Interfaces;
using Authorization.Infrastructure.DataAccess;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _appContext;
    private readonly ILogger<AuthRepository> _logger;
    private readonly IMapper _mapper;

    public AuthRepository(AppDbContext appContext, ILogger<AuthRepository> logger, IMapper mapper)
    {
        _appContext = appContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task Create(UserEntity entity,PasswordEntity password, CancellationToken cancellationToken)
    {
        
        await _appContext.Users.AddAsync(entity, cancellationToken);
        await _appContext.Passwords.AddAsync(password, cancellationToken);
        await _appContext.SaveChangesAsync(cancellationToken); ;
    }

    public async Task Delete(string userId, CancellationToken cancellationToken)
    {
        _appContext.Users.Remove((await _appContext.Users.FirstOrDefaultAsync(x=>x.Id == userId, cancellationToken))!);
        _appContext.Passwords.Remove((await _appContext.Passwords.FirstOrDefaultAsync(x=>x.UserId == userId, cancellationToken))!);
        await _appContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(UserDto entity, CancellationToken cancellationToken)
    { 
        var unchangedEntity = await _appContext.Users.Include(x=>x.PasswordEntity).FirstOrDefaultAsync(x => x.Id == entity.Id, cancellationToken);
        
        _mapper.Map(entity, unchangedEntity);
        await _appContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserEntity>GetUser(string userId, CancellationToken cancellationToken)
    {
        return (await _appContext.Users.FirstOrDefaultAsync(x=>x.Id == userId, cancellationToken))!;
    }

    public async Task<List<UserEntity>> GetUsers(CancellationToken cancellationToken)
    {
        return await _appContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<UserDto> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        var result = (await _appContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken));
        if (result == null)
        {
            return null;
        }
        var password = await _appContext.Passwords.FirstOrDefaultAsync(x => x.UserId == result.Id, cancellationToken);
        var response = new UserDto()
        {
            Id = result.Id,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.FirstName,
            Password = password.Password
        };
        return response;
    }

}