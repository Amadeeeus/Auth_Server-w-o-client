using Authorization.Application.UseCases.Commands.Delete;
using Authorization.Core.Interfaces;
using Authorization.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Authorization.Core.Handlers.Commands;

public class DeleteUserCommandHandler:IRequestHandler<DeleteUserRequest>
{
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IDistributedCache _cache;
    private readonly IAuthRepository _authRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger, IDistributedCache cache,
        IAuthRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _cache = cache;
        _authRepository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext.Request.Cookies["UserId"];
        _logger.LogInformation($"Delete user: {userId}");
        var cache = await _cache.GetStringAsync($"user: {userId}",cancellationToken);
        if (cache !=null)
        { 
            await _cache.RemoveAsync($"user: {userId}", cancellationToken);  
        }
        await _authRepository.Delete(userId!, cancellationToken);
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("Access");
        _httpContextAccessor.HttpContext.Response.Cookies.Delete("Refresh");
        _logger.LogInformation($"User {userId} deleted");
    }
}