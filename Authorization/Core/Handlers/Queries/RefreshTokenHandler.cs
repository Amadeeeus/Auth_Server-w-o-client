using Authorization.Application.UseCases.Queries.Get;
using Authorization.Core.Entities;
using Authorization.Core.Interfaces;
using Authorization.Core.JWT;
using AutoMapper;

namespace Authorization.Core.Handlers.Queries;

public class RefreshTokenHandler
{
    private readonly ILogger<RefreshTokenHandler> _logger;
    private readonly IAuthRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenerator _generator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenHandler(IJwtTokenGenerator generator,IAuthRepository repository, ILogger<RefreshTokenHandler> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _repository = repository;
        _generator = generator;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(RefreshToken request, CancellationToken ct)
    {
        var id = _httpContextAccessor.HttpContext!.Request.Cookies["userId"];
        var old = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];
        var check = _generator.GetPrincipalFromExpiredToken(old!);
        if (check is not null)
        {
            var user = await _repository.GetUser(id!,ct);
            var access = _generator.GenerateAccessToken(user.Email,id!);
            var refresh = _generator.GenerateRefreshToken(user.Email);
            user.RefreshToken = refresh;
            user.RefreshTokenExpiry =DateTime.Now.AddDays(10);
            await _repository.Update(_mapper.Map<UserDto>(user), ct);
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
}