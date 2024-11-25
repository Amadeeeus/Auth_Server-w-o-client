using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.JWT;
using Authorization.Infrastructure.Repositories;
using MediatR;

namespace Authorization.Core.Handlers.Commands;

public class LoginUserRequestHandler:IRequestHandler<LoginRequest, AuthResponceDTO>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<LoginUserRequestHandler> _logger;
    private readonly IJwtTokenGenerator? _access;
    private readonly IDistributedCacheRepository _cache;

    public LoginUserRequestHandler(IAuthRepository authRepository, ILogger<LoginUserRequestHandler> logger, IDistributedCacheRepository cache, IJwtTokenGenerator? access)
    {
        _authRepository = authRepository;
        _logger = logger;
        _cache = cache;
        _access = access;
    }

    public async Task<AuthResponceDTO> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _authRepository.GetUserByEmail(request.Entity.Email,cancellationToken);
        if (user.Email == null)
        {
            _logger.LogError("LoginHandler: Email address is invalid");
            throw new Exception("Invalid email");
            
        }
        var password = request.Entity.Password;
        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            _logger.LogError("LoginHandler: password is invalid");
            throw new Exception("Invalid password");
        }
        var result = new AuthResponceDTO()
        {
                AccessToken = _access!.GenerateAccessToken(user.Email, user.Id),
                RefreshToken = _access.GenerateRefreshToken(user.Email),
        };
        var cache = new UserDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Password = password,
            Email = user.Email
        };
            await _cache.AddCacheAsync(cache);
            return result;
        }
    }