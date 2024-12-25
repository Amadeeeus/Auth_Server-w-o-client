using System.Text.Json;
using Authorization.Application.UseCases.Commands.Create;
using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Core.DTOs;
using Authorization.Core.Interfaces;
using Authorization.Core.JWT;
using Authorization.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Authorization.Core.Handlers.Commands;

public class LoginUserRequestHandler:IRequestHandler<LoginUserRequest>
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<LoginUserRequestHandler> _logger;
    private readonly IJwtTokenGenerator? _access;
    private readonly IDistributedCacheRepository _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    public LoginUserRequestHandler(IAuthRepository authRepository,IMapper mapper,IHttpContextAccessor accessor, ILogger<LoginUserRequestHandler> logger, IDistributedCacheRepository cache, IJwtTokenGenerator? access)
    {
        _authRepository = authRepository;
        _logger = logger;
        _cache = cache;
        _access = access;
        _mapper = mapper;
        _httpContextAccessor = accessor;
    }

    public async Task Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _authRepository.GetUserByEmail(request.Entity.Email,cancellationToken);
        if (user.Email==null)
        {
            _logger.LogError("LoginHandler: Email address is invalid");
            throw new UnauthorizedAccessException("Invalid email");
        }
        _logger.LogError("LoginHandler: Email address found");
        var password = request.Entity.Password;
        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            _logger.LogError("LoginHandler: password is invalid");
            throw new UnauthorizedAccessException("Invalid password");
        }
        _httpContextAccessor.HttpContext.Response.Cookies.Append("userId", user.Id, new CookieOptions
        {
            Secure = true,
            Expires = DateTimeOffset.UtcNow.AddHours(10),
            SameSite = SameSiteMode.Lax
        });
        var getUser = _mapper.Map<GetUserDto>(user);
        var serializedUser = JsonSerializer.Serialize(getUser);
        _httpContextAccessor.HttpContext.Response.Cookies.Append("userInfo", serializedUser, new CookieOptions
        {
            Secure = false,
            Expires = DateTimeOffset.UtcNow.AddHours(10),
            SameSite = SameSiteMode.Lax
        });
        _logger.LogError("LoginHandler: Password correct");
        var access = _access!.GenerateAccessToken(user.Email, user.Id);
        var refresh =_access.GenerateRefreshToken(user.Email);
        _httpContextAccessor.HttpContext.Response.Cookies.Append("Access", access, new CookieOptions
        {
            Secure = false,
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddHours(10),
            SameSite = SameSiteMode.Lax
        });
        _httpContextAccessor.HttpContext.Response.Cookies.Append("Refresh", refresh, new CookieOptions
        {
            Secure = false,
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddHours(10),
            SameSite = SameSiteMode.Lax
        });
    }
    }