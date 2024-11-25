using Authorization.Application.UseCases.Commands.Create;
using Authorization.Core.DTOs;
using Authorization.Core.Entities;
using Authorization.Core.JWT;
using Authorization.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using Visus.Cuid;

namespace Authorization.Core.Handlers.Commands;

public class CreateUserCommandHandler:IRequestHandler<RegisterUserRequest, AuthResponceDTO>
{
  private readonly IDistributedCacheRepository _distributed;
  private readonly ILogger<CreateUserCommandHandler> _logger;
  private readonly IAuthRepository _authRepository;
  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IMapper _mapper;

  public CreateUserCommandHandler(IDistributedCacheRepository distributed, ILogger<CreateUserCommandHandler> logger,
    IAuthRepository repository, IJwtTokenGenerator access, IMapper mapper)
  {
    _distributed = distributed;
    _logger = logger;
    _authRepository = repository;
    _jwtTokenGenerator = access;
    _mapper = mapper;
  }

  public async Task<AuthResponceDTO> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
  {
    _logger.LogInformation("Registering user");
    var requestEmail = request.Entity.Email;
    var check = await _authRepository.GetUserByEmail(requestEmail, cancellationToken);
    if (check is not null)
    {
      throw new Exception($"User with email {requestEmail} already exists.");
    }
    var id = new Cuid2().ToString();
    var passwordId= new Cuid2().ToString();
    var access = _jwtTokenGenerator.GenerateAccessToken(requestEmail,id);
    var refresh = _jwtTokenGenerator.GenerateRefreshToken(requestEmail);
    var encrypted = BCrypt.Net.BCrypt.HashPassword(request.Entity.Password);
    var register =  _mapper.Map<UserEntity>(request);
    register.RefreshToken = refresh;
    register.Id = id;
    var password = new PasswordEntity {Id = passwordId,UserId = id, Password = encrypted};
    await _authRepository.Create(register,password, cancellationToken);
    var responce = new AuthResponceDTO
    {
      AccessToken = access,
      RefreshToken = refresh
    };
    _logger.LogInformation($"User with email {requestEmail} has been created.");
    await _distributed.AddCacheAsync(new UserDto()
    {
      Id = id,
      Email = requestEmail,
      FirstName = request.Entity.Firstname,
      LastName = request.Entity.Lastname,
      Password = password.Password
    });
    _logger.LogInformation($"User{requestEmail} has been cached.");
    return responce;
  }
}