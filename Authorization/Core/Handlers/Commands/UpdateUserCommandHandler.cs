using Authorization.Application.UseCases.Commands.Insert;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.Interfaces;
using Authorization.Infrastructure.Repositories;
using AutoMapper;
using MediatR;


namespace Authorization.Core.Handlers.Commands;

public class UpdateUserCommandHandler:IRequestHandler<PutUserRequest>
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IAuthRepository _authRepository;
    private readonly IDistributedCacheRepository _cache;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IAuthRepository authRepository,
        IDistributedCacheRepository cache, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _logger = logger;
        _authRepository = authRepository;
        _cache = cache;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task Handle(PutUserRequest request, CancellationToken cancellationToken)
    {
        var id = _httpContextAccessor.HttpContext!.Request.Cookies["userId"];
        var entity = _mapper.Map<UserEntity>(request);
        entity.Id = id!;
        var response = await _authRepository.Update(entity, cancellationToken);
        _logger.LogInformation($"User {id} has been successfully updated.");
        entity.RefreshToken = response.RefreshToken;
        var mapped = _mapper.Map<UserDto>(response);
        await _cache.AddCacheAsync(mapped);
        _logger.LogInformation($"User {id} cached.");
    }
}