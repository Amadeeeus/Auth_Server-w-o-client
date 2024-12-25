using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.Interfaces;
using Authorization.Core.JWT;
using Authorization.Infrastructure.Repositories;
using AutoMapper;
using MediatR;


namespace Authorization.Core.Handlers.Commands;

public class UpdateUserCommandHandler:IRequestHandler<PutUserRequest>
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IAuthRepository _authRepository;
    private readonly IDistributedCacheRepository _cache;
    private readonly IJwtTokenGenerator _generator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IAuthRepository authRepository,
        IDistributedCacheRepository cache, IHttpContextAccessor httpContextAccessor, IMapper mapper, IJwtTokenGenerator generator)
    {
        _generator = generator;
        _logger = logger;
        _authRepository = authRepository;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task Handle(PutUserRequest request, CancellationToken cancellationToken)
    {
        var id = _httpContextAccessor.HttpContext!.Request.Cookies["userId"];
        var entity = _mapper.Map<UserDto>(request);
        entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
        await _authRepository.Update(entity, cancellationToken);
        _logger.LogInformation($"User {id} has been successfully updated.");
        var access = _generator.GenerateAccessToken(id!, entity.Email);
        var refresh = _generator.GenerateRefreshToken(entity.Email);
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("Access");
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("Refresh");
        _httpContextAccessor.HttpContext.Response.Cookies.Append("Access", access, new CookieOptions
        {
            HttpOnly = true,
            Expires =  DateTime.Now.AddHours(20),
            SameSite = SameSiteMode.None
        });
        _httpContextAccessor.HttpContext.Response.Cookies.Append("Refresh", refresh, new CookieOptions
        {
            HttpOnly = true,
            Expires =  DateTime.Now.AddHours(20),
            SameSite = SameSiteMode.None
        });
        await _cache.AddCacheAsync(entity);
        _logger.LogInformation($"User {id} cached.");
    }
}